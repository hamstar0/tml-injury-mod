using HamstarHelpers.Helpers.Debug;
using Injury.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
				this.TemporaryMaxHpTimer = mymod.Config.TemporaryMaxHpChunkDrainTickRate;
			}
		}

		public TagCompound Save() {
			return new TagCompound { { "temp_max_hp", this.TemporaryMaxHp } };
		}


		////////////////

		public void UpdateBleeding( Player player ) {
			var mymod = InjuryMod.Instance;

			// Low hp (< %35) blood loss
			if( (float)player.statLife < (float)player.statLifeMax2 * mymod.Config.MaxHpPercentRemainingUntilBleeding ) {
				player.AddBuff( 30, 2 );
				Main.buffNoTimeDisplay[30] = true;  // Force bleeding to not render time
			} else {
				Main.buffNoTimeDisplay[30] = false;
			}
		}


		////////////////

		public bool IsPowerfulBlow( Player player, float damageWithCrit ) {
			var mymod = InjuryMod.Instance;
			return damageWithCrit > (float)player.statLifeMax2 * mymod.Config.MaxHpPercentLossForPowerfulBlowStagger;
		}


		////////////////

		public float ComputeFortifyScale( Player player ) {
			var mymod = InjuryMod.Instance;
			float amt = 1f;
			float add = 0f;
			var myplayer = player.GetModPlayer<InjuryPlayer>();

			if( player.FindBuffIndex( ModContent.BuffType<FortifiedBuff>() ) != -1 ) {
				add += mymod.Config.FortitudePotionHarmAddedBufferMultiplier - 1f;
			}
			if( myplayer.LifeVestPresence > 0 ) {
				add += mymod.Config.LifeVestHarmAddedBufferMultiplier - 1f;
			}

			if( mymod.IsDebugInfoMode() ) {
				DebugHelpers.Print( "fortify scale ", "" + (amt + add), 30 );
			}

			return amt + add;
		}


		////////////////
			
		public void ApplyRunImpairment( Player player ) {
			var mymod = InjuryMod.Instance;
			ImpactTraumaBuff.ApplyImpairment( mymod, player );
		}

		public void AfflictPowerfulBlowEffect( Player player ) {
			var mymod = InjuryMod.Instance;
			player.AddBuff( mymod.BuffType( "ImpactTrauma" ), 60 * 3 );
			player.AddBuff( BuffID.Weak, 60 * 6 );
		}


		////////////////

		public void InjuryVisualFX( Player player ) {
			var mymod = InjuryMod.Instance;
			Vector2 pos = player.position;
			int maxBlood = Main.rand.Next( 32, 48 );

			if( player.gravDir < 0 ) {
				pos.Y += player.height;
			}

			for( int i = 0; i < maxBlood; i++ ) {
				var velX = 2f - (Main.rand.NextFloat() * 4f);
				var velY = 2f - (Main.rand.NextFloat() * 4f);
				Dust.NewDust( pos, player.width, player.height, 5, velX, velY );
			}

			mymod.HealthLoss.AnimateHeartDrop();
		}

		public void InjuryFullFX( Player player ) {
			this.InjuryVisualFX( player );
			Main.PlaySound( SoundID.NPCHit16, player.position );
		}
	}
}
