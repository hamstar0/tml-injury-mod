using HamstarHelpers.PlayerHelpers;
using Injury.Projectiles;
using Terraria;
using Terraria.ID;


namespace Injury.Logic {
	partial class InjuryLogic {
		public void UpdateHarm( InjuryMod mymod, Player player ) {
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

		public bool CanBeHarmed( InjuryMod mymod, Player player, double damage, bool crit ) {
			double damage_with_crit = crit ? damage * 2 : damage;
			double max_hp_until_harm = (double)player.statLifeMax2 * mymod.Config.MaxHpPercentAsDamageAtFullHealthUntilHarm;

			return player.statLife < player.statLifeMax2    // Any amount of hurt
				|| damage_with_crit > max_hp_until_harm;
		}


		////////////////

		public float ComputeHarmFromDamage( InjuryMod mymod, Player player, double damage, bool crit ) {
			float damage_with_crit = crit ? (float)damage * 2f : (float)damage;
			float damage_clamped = damage_with_crit > player.statLife ? (float)(player.statLife + 1) : damage_with_crit;
			float harm = damage_clamped * mymod.Config.PercentOfDamageToUseAsInjury + mymod.Config.AdditionalInjuryPerDamagingHit;

			return harm;
		}

		public float ComputeHarmBufferCapacity( InjuryMod mymod, Player player ) {
			float hp_scale = 1f;

			if( mymod.Config.HighMaxHealthReducesInjury ) {
				hp_scale = 0.75f + ((float)player.statLifeMax / 400f);
			}

			float amt = (hp_scale < 1f ? 1f : hp_scale) * mymod.Config.HarmBufferCapacityBeforeReceivingInjury;
			amt *= this.ComputeFortifyScale( mymod, player );

			return amt;
		}

		public float ComputeHarmBufferPercent( InjuryMod mymod, Player player ) {
			return this.HiddenHarmBuffer / this.ComputeHarmBufferCapacity( mymod, player );
		}


		////////////////

		public void AfflictHarm( InjuryMod mymod, Player player, float harm ) {
			int min_hp = mymod.Config.LowestAllowedMaxHealth;
			bool is_injured = false;
			float injury_threshold = this.ComputeHarmBufferCapacity( mymod, player );

			if( player.statLifeMax <= min_hp ) { return; }

			this.HiddenHarmBuffer += harm;

			// When harm is sufficient to cause injury, transfer buffer to much ouch
			while( this.HiddenHarmBuffer >= injury_threshold && player.statLifeMax > min_hp ) {
				is_injured = true;
				this.HiddenHarmBuffer -= injury_threshold;
				player.statLifeMax -= mymod.Config.MaxHealthLostFromInjury;
			}

			// Enforce minimum health cap
			if( player.statLifeMax <= min_hp ) {
				is_injured = false;
				player.statLifeMax = min_hp;
				this.HiddenHarmBuffer = 0f;

				if( Main.netMode == 1 ) {
					NetMessage.SendData( MessageID.PlayerHealth, -1, player.whoAmI, null, player.whoAmI, 0f, 0f, 0f, 0, 0, 0 );
				}
			}

			if( is_injured ) {
				if( mymod.Config.BrokenHeartsDrop ) {
					bool mech = NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3;

					if( player.statLifeMax <= 415 || (mech && player.statLifeMax <= 400) ) {
						BleedingHeartProjectile.Spawn( player, mymod );
					} else {
						WanderingHeartProjectile.Spawn( player, mymod );
					}
				}

				this.InjuryFullFX( mymod, player );
			}
		}
	}
}
