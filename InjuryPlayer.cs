using HamstarHelpers.Helpers.PlayerHelpers;
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

		public override bool CloneNewInstances { get { return false; } }

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
			if( player.whoAmI != Main.myPlayer ) { return; }
			if( this.player.whoAmI != Main.myPlayer ) { return; }

			var mymod = (InjuryMod)this.mod;

			if( Main.netMode == 0 ) {   // Not server
				if( !mymod.ConfigJson.LoadFile() ) {
					mymod.ConfigJson.SaveFile();
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
		public override void PostHurt( bool pvp, bool quiet, double damage, int hitDirection, bool crit ) {
			var mymod = (InjuryMod)this.mod;
			double damageWithCrit = crit ? damage * 2 : damage;

			if( !mymod.Config.Enabled ) { return; }

			// Powerful blow stagger
			if( this.Logic.IsPowerfulBlow( player, (float)damageWithCrit ) ) {
				this.Logic.AfflictPowerfulBlowEffect( player );
			}
			
			if( !quiet && this.Logic.CanBeHarmed( player, damage, crit) ) {
				float harm = this.Logic.ComputeHarmFromDamage( player, damage, crit );
//Main.NewText("harmed: "+harm+" buffer: "+ this.HiddenHarmBuffer.ToString("N2")+" threshold: "+this.ComputeHarmBufferCapacity().ToString("N2") );
				this.Logic.AfflictHarm( player, harm );
			}
		}


		public override void PreUpdate() {
			var mymod = (InjuryMod)this.mod;
			if( !mymod.Config.Enabled ) { return; }

			this.Logic.UpdateBleeding( this.player );

			// Fall impact staggering
			if( this.player.velocity.Y == 0f ) {
				int dmg = PlayerHelpers.ComputeImpendingFallDamage( this.player );
				if( dmg != 0 ) {
					this.player.AddBuff( mod.BuffType("ImpactTrauma"), dmg * mymod.Config.FallLimpDurationMultiplier );
				}
			}

			this.Logic.UpdateHarm( this.player );
			this.Logic.UpdateTemporaryHealth( this.player );

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

						float harmBufferPercent = this.Logic.ComputeHarmBufferPercent( this.player );
						float harm = this.Logic.ComputeHarmBufferCapacity( player ) * (1f - harmBufferPercent);

						this.Logic.AfflictHarm( this.player, harm + 0.001f );
					}
				} else if( !player.dead ) {
					this.AmDead = false;
				}
			}
		}
		


		public override void PostUpdateRunSpeeds() {
			if( this.IsImpaired ) {
				this.Logic.ApplyRunImpairment( player );
				this.IsImpaired = false;
			}
		}
	}
}
