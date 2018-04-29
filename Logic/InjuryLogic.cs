using HamstarHelpers.DebugHelpers;
using Injury.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;


namespace Injury.Logic {
	partial class InjuryLogic {
		public float HiddenHarmBuffer { get; private set; }
		public int TemporaryMaxHp { get; private set; }

		private bool HasHealedInjury = false;
		private int TemporaryMaxHpTimer = 0;


		////////////////

		public InjuryLogic() {
			this.HiddenHarmBuffer = 0;
			this.TemporaryMaxHp = 0;
		}

		////////////////

		public void Load( InjuryMod mymod, TagCompound tags ) {
			if( tags.ContainsKey( "temp_max_hp" ) ) {
				this.TemporaryMaxHp = tags.GetInt( "temp_max_hp" );
				this.TemporaryMaxHpTimer = mymod.ServerConfig.TemporaryMaxHpChunkDrainTickRate;
			}
		}

		public TagCompound Save() {
			return new TagCompound { { "temp_max_hp", this.TemporaryMaxHp } };
		}


		////////////////

		public void UpdateBleeding( InjuryMod mymod, Player player ) {
			// Low hp (< %35) blood loss
			if( (float)player.statLife < (float)player.statLifeMax2 * mymod.ServerConfig.MaxHpPercentRemainingUntilBleeding ) {
				player.AddBuff( 30, 2 );
				Main.buffNoTimeDisplay[30] = true;  // Force bleeding to not render time
			} else {
				Main.buffNoTimeDisplay[30] = false;
			}
		}


		////////////////

		public bool IsPowerfulBlow( InjuryMod mymod, Player player, float damage_with_crit ) {
			return damage_with_crit > (float)player.statLifeMax2 * mymod.ServerConfig.MaxHpPercentLossForPowerfulBlowStagger;
		}


		////////////////

		public float ComputeFortifyScale( InjuryMod mymod, Player player ) {
			float amt = 1f;
			float add = 0f;
			var modplayer = player.GetModPlayer<InjuryPlayer>();

			if( player.FindBuffIndex( mymod.BuffType<FortifiedBuff>() ) != -1 ) {
				add += mymod.ServerConfig.FortitudePotionHarmBufferMultiplier - 1f;
			}
			if( modplayer.LifeVestPresence > 0 ) {
				add += mymod.ServerConfig.LifeVestHarmBufferMultiplier - 1f;
			}

			if( mymod.ServerConfig.DebugModeInfo ) {
				DebugHelpers.SetDisplay( "fortify scale ", "" + ( amt + add ), 30 );
			}

			return amt + add;
		}


		////////////////
			
		public void ApplyRunImpairment( InjuryMod mymod, Player player ) {
			ImpactTraumaBuff.ApplyImpairment( mymod, player );
		}

		public void AfflictPowerfulBlowEffect( InjuryMod mymod, Player player ) {
			player.AddBuff( mymod.BuffType( "ImpactTrauma" ), 60 * 3 );
			player.AddBuff( BuffID.Weak, 60 * 6 );
		}


		////////////////

		public void InjuryVisualFX( InjuryMod mymod, Player player ) {
			Vector2 pos = player.position;
			int max_blood = Main.rand.Next( 32, 48 );

			if( player.gravDir < 0 ) {
				pos.Y += player.height;
			}

			for( int i = 0; i < max_blood; i++ ) {
				var vel_x = 2f - (Main.rand.NextFloat() * 4f);
				var vel_y = 2f - (Main.rand.NextFloat() * 4f);
				Dust.NewDust( pos, player.width, player.height, 5, vel_x, vel_y );
			}

			mymod.HealthLoss.AnimateHeartDrop();
		}

		public void InjuryFullFX( InjuryMod mymod, Player player ) {
			this.InjuryVisualFX( mymod, player );
			Main.PlaySound( SoundID.NPCHit16, player.position );
		}
	}
}
