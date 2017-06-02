using Injury.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Utils;


namespace Injury {
	public class InjuryPlayer : ModPlayer {
		public float HiddenHarmBuffer { get; private set; }
		public bool IsImpaired;
		public int TemporaryMaxHp { get; private set; }

		private bool HasHealedInjury = false;
		private int TemporaryMaxHpTimer = 0;


		////////////////

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (InjuryPlayer)clone;

			myclone.HiddenHarmBuffer = this.HiddenHarmBuffer;
			myclone.IsImpaired = this.IsImpaired;
			myclone.TemporaryMaxHp = this.TemporaryMaxHp;
			myclone.HasHealedInjury = this.HasHealedInjury;
			myclone.TemporaryMaxHpTimer = this.TemporaryMaxHpTimer;
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (InjuryMod)this.mod;

			if( Main.netMode != 2 ) {   // Not server
				if( player.whoAmI == this.player.whoAmI ) {
					if( !mymod.Config.LoadFile() ) {
						mymod.Config.SaveFile();
					}

					if( Main.netMode == 1 ) {   // Client
						InjuryNetProtocol.SendSettingsRequestFromClient( mymod, player );
					}
				}
			}
		}

		public override void Load( TagCompound tags ) {
			var mymod = (InjuryMod)this.mod;

			if( !mymod.Config.LoadFile() ) {
				mymod.Config.SaveFile();
			}
			
			if( tags.ContainsKey( "temp_max_hp" ) ) {
				int temp_max_hp = tags.GetInt( "temp_max_hp" );
				this.TemporaryMaxHp = temp_max_hp;
				this.TemporaryMaxHpTimer = mymod.Config.Data.TemporaryMaxHpChunkDrainTickRate;
			}
		}

		public override TagCompound Save() {
			return new TagCompound { {"temp_max_hp", this.TemporaryMaxHp} };
		}

		////////////////

		//public override bool PreHurt( bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource ) {
		public override void PostHurt( bool pvp, bool quiet, double damage, int hit_direction, bool crit ) {
			var mymod = (InjuryMod)this.mod;
			double damage_with_crit = crit ? damage * 2 : damage;

			if( !mymod.Config.Data.Enabled ) { return; }

			// Powerful blow stagger
			if( (float)damage_with_crit > (float)this.player.statLifeMax2 * mymod.Config.Data.MaxHpPercentLossForPowerfulBlowStagger ) {
				this.player.AddBuff( mod.BuffType("ImpactTrauma"), 3 );
				this.player.AddBuff( 33, 6 );	// Weak
			}

			if( !quiet && this.CanBeHarmed(damage, crit) ) {
				float harm = this.ComputeHarm( damage, crit );
//Main.NewText("harmed: "+harm+" buffer: "+ this.HiddenHarmBuffer.ToString("N2")+" threshold: "+this.ComputeHarmBufferCapacity().ToString("N2") );
				this.AfflictHarm( harm );
			}
		}


		public override void PreUpdate() {
			var mymod = (InjuryMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			// Low hp (< %35) blood loss
			if( (float)this.player.statLife < (float)this.player.statLifeMax2 * mymod.Config.Data.MaxHpPercentRemainingUntilBleeding ) {
				this.player.AddBuff( 30, 2 );
				Main.buffNoTimeDisplay[30] = true;	// Force bleeding to not render time
			} else {
				Main.buffNoTimeDisplay[30] = false;
			}

			// Fall impact staggering
			if( this.player.velocity.Y == 0f ) {
				int dmg = PlayerHelper.ComputeImpendingFallDamage( this.player );
				if( dmg != 0 ) {
					this.player.AddBuff( mod.BuffType("ImpactTrauma"), dmg * mymod.Config.Data.FallLimpDurationMultiplier );
				}
			}

			// Erode harm gradually
			this.HiddenHarmBuffer -= mymod.Config.Data.InjuryBufferHealPerSecond;
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

			// Erode temporary max hp
			if( this.TemporaryMaxHp > 0 ) {
				if( this.TemporaryMaxHpTimer == 0 ) {
					this.TemporaryMaxHpTimer = mymod.Config.Data.TemporaryMaxHpChunkDrainTickRate;
					this.TemporaryMaxHp -= 5;

					if( this.player.statLifeMax > mymod.Config.Data.LowestAllowedMaxHealth ) {
						this.player.statLifeMax -= 5;
						this.InjuryVisualFX();
					} else {
						this.TemporaryMaxHpTimer = 0;
						this.TemporaryMaxHp = 0;
					}
				} else {
					this.TemporaryMaxHpTimer -= 1;
				}
			}

			if( DebugHelper.DEBUGMODE ) {
				DebugHelper.Display["wear"] = this.HiddenHarmBuffer.ToString("N2") + " : " + this.ComputeHarmBufferCapacity().ToString("N2");
			}
		}


		public override void PostUpdateRunSpeeds() {
			if( this.IsImpaired ) {
				Buffs.ImpactTrauma.ApplyImpairment( (InjuryMod)this.mod, this.player );
				this.IsImpaired = false;
			}
		}

		////////////////

		public bool CanTemporaryInjuryHeal( int amt ) {
			return this.player.statLifeMax < 400;
		}
		
		public bool TemporaryInjuryHeal( int amt ) {
			var mymod = (InjuryMod)this.mod;

			this.player.statLifeMax += amt;
			this.TemporaryMaxHp += amt;
			this.TemporaryMaxHpTimer = mymod.Config.Data.TemporaryMaxHpChunkDrainTickRate;

			return true;
		}

		////////////////

		public bool CanBeHarmed( double damage, bool crit ) {
			var mymod = (InjuryMod)this.mod;
			double damage_with_crit = crit ? damage * 2 : damage;
			double max_hp_until_harm = (double)player.statLifeMax2 * mymod.Config.Data.MaxHpPercentAsDamageAtFullHealthUntilHarm;
			
			return player.statLife < player.statLifeMax2	// Any amount of hurt
				|| damage_with_crit > max_hp_until_harm;
		}

		public float ComputeHarm( double damage, bool crit ) {
			var mymod = (InjuryMod)this.mod;
			var data = mymod.Config.Data;

			float damage_with_crit = crit ? (float)damage * 2f : (float)damage;
			float damage_clamped = damage_with_crit > this.player.statLife ? (float)(this.player.statLife + 1) : damage_with_crit;
			float harm = damage_clamped * data.PercentOfDamageToUseAsInjury + data.AdditionalInjuryPerDamagingHit;

			return harm;
		}

		public float ComputeHarmBufferCapacity() {
			var mymod = (InjuryMod)this.mod;
			var data = mymod.Config.Data;
			float min = 1f;

			if( data.HighMaxHealthReducesInjury ) {
				min = 0.75f + ((float)this.player.statLifeMax / 400f);
			}

			return (min < 1f ? 1f : min) * data.HarmBufferCapacityBeforeReceivingInjury;
		}
		

		public void AfflictHarm( float harm ) {
			var mymod = (InjuryMod)this.mod;
			int min_hp = mymod.Config.Data.LowestAllowedMaxHealth;
			bool is_injured = false;
			float injury_threshold = this.ComputeHarmBufferCapacity();

			if( this.player.statLifeMax <= min_hp ) { return; }
			
			this.HiddenHarmBuffer += harm;

			// When harm is sufficient to cause injury, transfer buffer to much ouch
			while( this.HiddenHarmBuffer >= injury_threshold && this.player.statLifeMax > min_hp ) {
				is_injured = true;
				this.HiddenHarmBuffer -= injury_threshold;
				this.player.statLifeMax -= mymod.Config.Data.MaxHealthLostFromInjury;
			}

			// Enforce minimum health cap
			if( this.player.statLifeMax <= min_hp ) {
				is_injured = false;
				this.player.statLifeMax = min_hp;
				this.HiddenHarmBuffer = 0f;
			}

			if( is_injured ) {
				if( mymod.Config.Data.BrokenHeartsDrop ) {
					BleedingHeartProjectile.Spawn( this.player, this.mod );
				}
				
				this.InjuryFullFX();
			}
		}


		public void InjuryVisualFX() {
			var pos = new Vector2( this.player.position.X, this.player.position.Y );
			var mymod = (InjuryMod)this.mod;
			int max_blood = Main.rand.Next( 32, 48 );

			if( this.player.gravDir < 0 ) {
				pos.Y += this.player.height;
			}

			for( int i = 0; i < max_blood; i++ ) {
				var vel_x = 2f - (Main.rand.NextFloat() * 4f);
				var vel_y = 2f - (Main.rand.NextFloat() * 4f);
				Dust.NewDust( pos, this.player.width, this.player.height, 5, vel_x, vel_y );
			}

			mymod.AnimateHeartDrop();
		}

		public void InjuryFullFX() {
			this.InjuryVisualFX();
			Main.PlaySound( SoundID.NPCHit16, this.player.position );
		}
	}
}
