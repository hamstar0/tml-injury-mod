using HamstarHelpers.PlayerHelpers;
using Injury.Logic;
using Injury.NetProtocol;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;


namespace Injury {
	class InjuryPlayer : ModPlayer {
		public InjuryLogic Logic { get; private set; }

		public bool IsImpaired = false;
		public int HeartstringsEffectDuration = 0;
		public int LifeVestPresence = 0;

		private bool AmDead = false;


		////////////////

		public override void Initialize() {
			this.Logic = new InjuryLogic();
		}

		public override void clientClone( ModPlayer clone ) {
			base.clientClone( clone );
			var myclone = (InjuryPlayer)clone;

			myclone.Logic = this.Logic;
			myclone.IsImpaired = this.IsImpaired;
			myclone.HeartstringsEffectDuration = this.HeartstringsEffectDuration;
			myclone.LifeVestPresence = this.LifeVestPresence;
			myclone.AmDead = this.AmDead;
		}


		////////////////

		public override void SyncPlayer( int to_who, int from_who, bool new_player ) {
			var mymod = (InjuryMod)this.mod;

			if( Main.netMode == 1 ) {
				if( new_player ) {
					ClientPacketHandlers.SendSettingsRequest( mymod );
				}
			}
		}


		public override void OnEnterWorld( Player player ) {
			var mymod = (InjuryMod)this.mod;

			if( player.whoAmI == this.player.whoAmI ) {
				if( Main.netMode == 0 ) {   // Not server
					if( !mymod.JsonConfig.LoadFile() ) {
						mymod.JsonConfig.SaveFile();
					}
				}
			}
		}

		////////////////

		public override void Load( TagCompound tags ) {
			var mymod = (InjuryMod)this.mod;

			this.Logic.Load( (InjuryMod)this.mod, tags );
		}

		public override TagCompound Save() {
			return this.Logic.Save();
		}


		////////////////

		//public override bool PreHurt( bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource ) {
		public override void PostHurt( bool pvp, bool quiet, double damage, int hit_direction, bool crit ) {
			var mymod = (InjuryMod)this.mod;
			double damage_with_crit = crit ? damage * 2 : damage;

			if( !mymod.Config.Enabled ) { return; }

			// Powerful blow stagger
			if( this.Logic.IsPowerfulBlow( mymod, player, (float)damage_with_crit ) ) {
				this.Logic.AfflictPowerfulBlowEffect( mymod, player );
			}

			if( !quiet && this.Logic.CanBeHarmed( mymod, player, damage, crit ) ) {
				float harm = this.Logic.ComputeHarmFromDamage( mymod, player, damage, crit );
				//Main.NewText("harmed: "+harm+" buffer: "+ this.HiddenHarmBuffer.ToString("N2")+" threshold: "+this.ComputeHarmBufferCapacity().ToString("N2") );
				this.Logic.AfflictHarm( mymod, player, harm );
			}
		}


		public override void PreUpdate() {
			var mymod = (InjuryMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			this.Logic.UpdateBleeding( mymod, this.player );

			// Fall impact staggering
			if( this.player.velocity.Y == 0f ) {
				int dmg = PlayerHelpers.ComputeImpendingFallDamage( this.player );
				if( dmg != 0 ) {
					this.player.AddBuff( mod.BuffType( "ImpactTrauma" ), dmg * mymod.Config.FallLimpDurationMultiplier );
				}
			}

			this.Logic.UpdateHarm( mymod, this.player );
			this.Logic.UpdateTemporaryHealth( mymod, this.player );

			if( this.HeartstringsEffectDuration > 0 ) {
				this.HeartstringsEffectDuration--;
			}
			if( this.LifeVestPresence > 0 ) {
				this.LifeVestPresence--;
			}

			if( mymod.Config.InjuryOnDeath ) {
				if( !this.AmDead ) {
					if( player.dead ) {
						this.AmDead = true;

						float harm_buffer_percent = this.Logic.ComputeHarmBufferPercent( mymod, this.player );
						float harm = this.Logic.ComputeHarmBufferCapacity( mymod, player ) * ( 1f - harm_buffer_percent );

						this.Logic.AfflictHarm( mymod, this.player, harm + 0.001f );
					}
				} else if( !player.dead ) {
					this.AmDead = false;
				}
			}
		}



		public override void PostUpdateRunSpeeds() {
			if( this.IsImpaired ) {
				this.Logic.ApplyRunImpairment( (InjuryMod)this.mod, player );
				this.IsImpaired = false;
			}
		}
	}
}
