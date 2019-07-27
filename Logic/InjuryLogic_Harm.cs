using HamstarHelpers.Helpers.Players;
using Injury.Projectiles;
using Terraria;
using Terraria.ID;


namespace Injury.Logic {
	partial class InjuryLogic {
		public void UpdateHarm( Player player ) {
			var mymod = InjuryMod.Instance;

			// Erode harm gradually
			this.HiddenHarmBuffer -= mymod.Config.InjuryBufferHealPerSecond;
			if( this.HiddenHarmBuffer < 0f ) { this.HiddenHarmBuffer = 0f; }
			if( player.dead ) { this.HiddenHarmBuffer = 0f; }

			// Remove harm erode with nurse heal
			if( PlayerNPCHelpers.HasUsedNurse( player ) ) {
				if( !this.HasHealedInjury ) {
					this.HasHealedInjury = true;
					this.HiddenHarmBuffer = 0;
				}
			} else {
				this.HasHealedInjury = false;
			}
		}


		////////////////

		public bool CanBeHarmed( Player player, double damage, bool crit ) {
			var mymod = InjuryMod.Instance;
			double damageWithCrit = crit ? damage * 2 : damage;
			double maxHpUntilHarm = (double)player.statLifeMax2 * mymod.Config.MaxHpPercentAsDamageAtFullHealthUntilHarm;

			return player.statLife < player.statLifeMax2    // Any amount of hurt
				|| damageWithCrit > maxHpUntilHarm;
		}


		////////////////

		public float ComputeHarmFromDamage( Player player, double damage, bool crit ) {
			var mymod = InjuryMod.Instance;
			float damageWithCrit = crit ? (float)damage * 2f : (float)damage;
			float damageClamped = damageWithCrit > player.statLife ? (float)(player.statLife + 1) : damageWithCrit;
			float harm = damageClamped * mymod.Config.PercentOfDamageToUseAsInjury + mymod.Config.AdditionalInjuryPerDamagingHit;

			return harm;
		}

		public float ComputeHarmBufferCapacity( Player player ) {
			var mymod = InjuryMod.Instance;
			float hpScale = 1f;

			if( mymod.Config.HighMaxHealthReducesInjury ) {
				hpScale = 0.75f + ((float)player.statLifeMax / 400f);
			}

			float amt = (hpScale < 1f ? 1f : hpScale) * mymod.Config.HarmBufferCapacityBeforeReceivingInjury;
			amt *= this.ComputeFortifyScale( player );

			return amt;
		}

		public float ComputeHarmBufferPercent( Player player ) {
			var mymod = InjuryMod.Instance;
			return this.HiddenHarmBuffer / this.ComputeHarmBufferCapacity( player );
		}


		////////////////

		public void AfflictHarm( Player player, float harm ) {
			var mymod = InjuryMod.Instance;
			int minHp = mymod.Config.LowestAllowedMaxHealth;
			bool isInjured = false;
			float injuryThreshold = this.ComputeHarmBufferCapacity(  player );

			if( player.statLifeMax <= minHp ) { return; }

			this.HiddenHarmBuffer += harm;

			// When harm is sufficient to cause injury, transfer buffer to much ouch
			while( this.HiddenHarmBuffer >= injuryThreshold && player.statLifeMax > minHp ) {
				isInjured = true;
				this.HiddenHarmBuffer -= injuryThreshold;
				player.statLifeMax -= mymod.Config.MaxHealthLostFromInjury;
			}

			// Enforce minimum health cap
			if( player.statLifeMax <= minHp ) {
				isInjured = false;
				player.statLifeMax = minHp;
				this.HiddenHarmBuffer = 0f;

				if( Main.netMode == 1 ) {
					NetMessage.SendData( MessageID.PlayerHealth, -1, player.whoAmI, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
				}
			}

			if( isInjured ) {
				if( mymod.Config.BrokenHeartsDrop ) {
					bool mech = NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3;

					if( player.statLifeMax <= 415 || (mech && player.statLifeMax <= 400) ) {
						BleedingHeartProjectile.Spawn( player );
					} else {
						WanderingHeartProjectile.Spawn( player );
					}
				}

				this.InjuryFullFX( player );
			}
		}
	}
}
