using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Utils;
using Utils.JsonConfig;
using System.IO;
using System;


namespace Injury {
	public class ConfigurationData {
		public string VersionSinceUpdate = "";
		public float PercentOfDamageToUseAsHarm = 0.075f;
		public int FallLimpDurationMultiplier = 9;
		public float FallLimpSpeedMultiplier = 0.45f;
		public float FallLimpJumpMultiplier = 0.35f;
		public int LowestAllowedMaxHealth = 20;
		public float HarmHealPerSecond = 1f / (60f * 75f);	// 1 hp every 75 seconds
		public float BandOfLifeHarmHealPerSecond = 1f / (60f * 30f); // 1 hp every 30 seconds
		public float AdditionalHarmPerDamagingHit = 0f;
		public float HarmBeforeReceivingInjury = 5f;
		public bool HighMaxHealthReducesReceivedHarm = true;
		public int MaxHealthLostFromInjury = 5;
		public bool BrokenHeartsDrop = true;
	}



	public class InjuryMod : Mod {
		public readonly static Version ConfigVersion = new Version(1, 8, 1);
		public static JsonConfig<ConfigurationData> Config { get; private set; }
		public Texture2D HeartTex { get; private set; }


		public InjuryMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			string filename = "Injury Config.json";
			InjuryMod.Config = new JsonConfig<ConfigurationData>(filename, new ConfigurationData());
		}

		public override void Load() {
			if( Main.netMode != 2 ) {	// Not server
				this.HeartTex = ModLoader.GetTexture( "Terraria/Heart" );
			}

			if( !InjuryMod.Config.Load() ) {
				InjuryMod.Config.Save();
			} else {
				Version vers_since = InjuryMod.Config.Data.VersionSinceUpdate != "" ?
					new Version( InjuryMod.Config.Data.VersionSinceUpdate ) :
					new Version();

				if( vers_since < InjuryMod.ConfigVersion ) {
					ErrorLogger.Log( "Injury config updated to " + InjuryMod.ConfigVersion.ToString() );
					InjuryMod.Config.Data.VersionSinceUpdate = InjuryMod.ConfigVersion.ToString();

					if( vers_since < new Version( 1, 8, 1 ) ) {
						InjuryMod.Config.Data.BandOfLifeHarmHealPerSecond = new ConfigurationData().BandOfLifeHarmHealPerSecond;
					}

					InjuryMod.Config.Save();
				}
			}
		}



		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			InjuryNetProtocol.RoutePacket( this, reader );
		}

		public override void AddRecipeGroups() {
			RecipeGroup group = new RecipeGroup( () => Lang.misc[37] + " Evil Biome Boss Drop", new int[] { 86, 1329 } );
			RecipeGroup.RegisterGroup( "InjuryMod:EvilBiomeBossDrop", group );
		}



		////////////////

		private void AnimateHeartDrop( SpriteBatch sb, int frame, int maxframes ) {
			int x = 0;
			int y = 0;

			PlayerHelper.GetTopHeartPosition( Main.player[Main.myPlayer], ref x, ref y );
			y += frame * 2;

			Rectangle rect = new Rectangle( x, y, this.HeartTex.Width, this.HeartTex.Height );
			float alpha = (0.5f - (frame / maxframes) / 2);

			sb.Draw( this.HeartTex, rect, Color.White * alpha );
		}


		private bool IsAnimatingHeartDrop = false;
		private int HeartDropAnimation = 0;

		public void AnimateHeartDrop() {
			this.IsAnimatingHeartDrop = true;
		}


		public override void PostDrawInterface( SpriteBatch sb ) {
			if( this.IsAnimatingHeartDrop ) {
				this.AnimateHeartDrop( sb, this.HeartDropAnimation++, 32 );
			
				if( this.HeartDropAnimation > 16 ) {
					this.HeartDropAnimation = 0;
					this.IsAnimatingHeartDrop = false;
				}
			}

			Debug.PrintToBatch( sb );
		}
	}
}
