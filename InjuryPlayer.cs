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
		}

		public override void OnEnterWorld( Player player ) {
			var mymod = (InjuryMod)this.mod;

			if( Main.netMode != 2 ) {   // Not server
				if( player.whoAmI == this.player.whoAmI ) {
					if( !mymod.Config.LoadFile() ) {
						mymod.Config.SaveFile();
					}

					if( Main.netMode == 1 ) {   // Client
						ClientPacketHandlers.SendSettingsRequestFromClient( mymod, player );
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

			if( !mymod.Config.Data.Enabled ) { return; }

			// Powerful blow stagger
			if( this.Logic.IsPowerfulBlow( mymod, player, (float)damage_with_crit ) ) {
				this.Logic.AfflictPowerfulBlowEffect( mymod, player );
			}
			
			if( !quiet && this.Logic.CanBeHarmed( mymod, player, damage, crit) ) {
				float harm = this.Logic.ComputeHarm( mymod, player, damage, crit );
//Main.NewText("harmed: "+harm+" buffer: "+ this.HiddenHarmBuffer.ToString("N2")+" threshold: "+this.ComputeHarmBufferCapacity().ToString("N2") );
				this.Logic.AfflictHarm( mymod, player, harm );
			}
		}


		public override void PreUpdate() {
			var mymod = (InjuryMod)this.mod;
			if( !mymod.Config.Data.Enabled ) { return; }

			this.Logic.UpdateBleeding( mymod, this.player );

			// Fall impact staggering
			if( this.player.velocity.Y == 0f ) {
				int dmg = PlayerHelpers.ComputeImpendingFallDamage( this.player );
				if( dmg != 0 ) {
					this.player.AddBuff( mod.BuffType("ImpactTrauma"), dmg * mymod.Config.Data.FallLimpDurationMultiplier );
				}
			}

			this.Logic.UpdateHarm( mymod, this.player );
			this.Logic.UpdateTemporaryHealth( mymod, this.player );

			if( this.HeartstringsEffectDuration > 0 ) {
				this.HeartstringsEffectDuration--;
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
