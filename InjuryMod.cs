using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using System.IO;
using HamstarHelpers.Utilities.Config;
using HamstarHelpers.HudHelpers;
using Injury.NetProtocol;


namespace Injury {
	public class InjuryMod : Mod {
		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-injury-mod"; } }

		public static string ConfigRelativeFilePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + InjuryConfigDefaults.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( InjuryMod.Instance != null && Main.netMode != 1 ) {
				InjuryMod.Instance.Config.LoadFile();
			}
		}

		public static InjuryMod Instance { get; private set; }


		////////////////

		public JsonConfig<InjuryConfigDefaults> Config { get; private set; }

		public Texture2D HeartTex { get; private set; }


		////////////////

		public InjuryMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.Config = new JsonConfig<InjuryConfigDefaults>( InjuryConfigDefaults.ConfigFileName,
				ConfigurationDataBase.RelativePath, new InjuryConfigDefaults() );
		}

		////////////////

		public override void Load() {
			InjuryMod.Instance = this;

			if( Main.netMode != 2 ) {   // Not server
				this.HeartTex = ModLoader.GetTexture( "Terraria/Heart" );
			}

			this.LoadConfig();
		}

		private void LoadConfig() {
			if( !this.Config.LoadFile() ) {
				this.Config.SaveFile();
			}

			if( this.Config.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Injury updated to " + InjuryConfigDefaults.ConfigVersion.ToString() );
				this.Config.SaveFile();
			}
		}


		public override void Unload() {
			InjuryMod.Instance = null;
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int player_who ) {
			if( Main.netMode == 1 ) {
				ClientPacketHandlers.RoutePacket( this, reader );
			} else if( Main.netMode == 2 ) {
				ServerPacketHandlers.RoutePacket( this, reader, player_who );
			}
		}

		////////////////

		public override void AddRecipeGroups() {
			RecipeGroup group = new RecipeGroup( () => Lang.misc[37] + " Evil Biome Boss Drop", new int[] { 86, 1329 } );
			RecipeGroup.RegisterGroup( "InjuryMod:EvilBiomeBossDrop", group );
		}
		

		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			if( this.IsAnimatingHeartDrop ) {
				this.AnimateHeartDrop( sb, this.HeartDropAnimation++, 32 );
			
				if( this.HeartDropAnimation > 16 ) {
					this.HeartDropAnimation = 0;
					this.IsAnimatingHeartDrop = false;
				}
			}
		}


		////////////////

		private void AnimateHeartDrop( SpriteBatch sb, int frame, int maxframes ) {
			int x = 0;
			int y = 0;

			HudHelpers.GetTopHeartPosition( Main.player[Main.myPlayer], ref x, ref y );
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

	}
}
