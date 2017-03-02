using Injury.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Utils;

namespace Injury {
	class InjuryPlayer : ModPlayer {
		public float HiddenHarmBuffer { get; private set; }
		public bool IsImpaired;

		private bool HasHealedInjury = false;


		////////////////

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (InjuryPlayer)clone;

			myclone.HiddenHarmBuffer = this.HiddenHarmBuffer;
			myclone.IsImpaired = this.IsImpaired;
			myclone.HasHealedInjury = this.HasHealedInjury;
		}

		public override void OnEnterWorld( Player player ) {
			if( Main.netMode == 1 ) {   // Client
				if( player.whoAmI == this.player.whoAmI ) {
					if( !InjuryMod.Config.Load() ) {
						InjuryMod.Config.Save();
					}

					InjuryNetProtocol.SendSettingsRequestFromClient( this.mod, player );
				}
			}
		}


		////////////////

		public override void PostHurt( bool pvp, bool quiet, double damage, int hitDirection, bool crit ) {
		//public override bool PreHurt( bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource ) {
			// Powerful blow stagger
			if( (float)damage > (float)this.player.statLifeMax2 * 0.25f ) {
				this.player.AddBuff(mod.BuffType("ImpactTrauma"), 3 );
				this.player.AddBuff( 33, 6 );	// Weak
			}

			if( !quiet ) {
				double max_dmg = damage > this.player.statLife ? this.player.statLife + 1 : damage;
				float harm = this.ComputeHarm( max_dmg, crit );
				this.Harm( harm );
			}
			
			//return base.PreHurt( pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource );
		}


		public override void PreUpdate() {
			// Low hp (< %35) blood loss
			if( (float)this.player.statLife < (float)this.player.statLifeMax2 * 0.35f ) {
				this.player.AddBuff( 30, 2 );
				Main.buffNoTimeDisplay[30] = true;	// Force bleeding to not render time
			} else {
				Main.buffNoTimeDisplay[30] = false;
			}

			// Fall impact staggering
			if( this.player.velocity.Y == 0f ) {
				int dmg = PlayerHelper.ComputeImpendingFallDamage( this.player );
				if( dmg != 0 ) {
					this.player.AddBuff( mod.BuffType("ImpactTrauma"), dmg * InjuryMod.Config.Data.FallLimpDurationMultiplier );
				}
			}

			// Erode harm gradually
			this.HiddenHarmBuffer -= InjuryMod.Config.Data.HarmHealPerSecond;
			if( this.HiddenHarmBuffer < 0f ) { this.HiddenHarmBuffer = 0f; }
			if( this.player.dead ) { this.HiddenHarmBuffer = 0f; }

			// Remove harm erode with nurse heal
			if( PlayerHelper.HasUsedNurse( this.player ) ) {
				if( !this.HasHealedInjury ) {
					this.HasHealedInjury = true;
					this.HiddenHarmBuffer = 0;
				}
			} else {
				this.HasHealedInjury = false;
			}

			if( Debug.DEBUGMODE ) {
				Debug.Display["wear"] = this.HiddenHarmBuffer.ToString("N2") + " : " + this.ComputeHarmToInjuryThreshold().ToString("N2");
			}
		}


		public override void PostUpdateRunSpeeds() {
			if( this.IsImpaired ) {
				Buffs.ImpactTrauma.ApplyImpairment( this.player );
				this.IsImpaired = false;
			}
		}

		////////////////
		
		public float ComputeHarm( double damage, bool crit ) {
			var data = InjuryMod.Config.Data;
			float harm = (float)damage * data.PercentOfDamageToUseAsHarm + data.AdditionalHarmPerDamagingHit;
			return crit ? harm * 2f : harm;
		}

		public float ComputeHarmToInjuryThreshold() {
			var data = InjuryMod.Config.Data;
			float min = 1f;

			if( data.HighMaxHealthReducesReceivedHarm ) {
				min = 0.75f + ((float)this.player.statLifeMax / 400f);
			}

			return (min < 1f ? 1f : min) * data.HarmBeforeReceivingInjury;
		}
		

		public void Harm( float harm ) {
			int min_hp = InjuryMod.Config.Data.LowestAllowedMaxHealth;
			bool is_injured = false;

			if( this.player.statLifeMax <= min_hp ) { return; }
			
			this.HiddenHarmBuffer += harm;

			while( this.HiddenHarmBuffer >= this.ComputeHarmToInjuryThreshold() && this.player.statLifeMax > min_hp ) {
				is_injured = true;
				this.HiddenHarmBuffer -= this.ComputeHarmToInjuryThreshold();
				this.player.statLifeMax -= InjuryMod.Config.Data.MaxHealthLostFromInjury;
			}

			// Enforce minimum health cap
			if( this.player.statLifeMax <= min_hp ) {
				this.player.statLifeMax = min_hp;
				this.HiddenHarmBuffer = 0f;
			}

			if( is_injured ) {
				if( InjuryMod.Config.Data.BrokenHeartsDrop ) {
					BleedingHeartProjectile.Spawn( this.player, this.mod );
				}

				InjuryMod mymod = (InjuryMod)this.mod;
				mymod.AnimateHeartDrop();
				this.Ouch();
			}
		}


		public void Ouch() {
			var pos = new Vector2( this.player.position.X, this.player.position.Y );
			if( this.player.gravDir < 0 ) {
				pos.Y += this.player.height;
			}

			int max_blood = Main.rand.Next( 32, 48 );
			for( int i=0; i < max_blood; i++ ) {
				var vel_x = 2f - (Main.rand.NextFloat() * 4f);
				var vel_y = 2f - (Main.rand.NextFloat() * 4f);
				Dust.NewDust( pos, this.player.width, this.player.height, 5, vel_x, vel_y );
			}

			Main.PlaySound( SoundID.NPCHit16, this.player.position );
		}
	}
}
