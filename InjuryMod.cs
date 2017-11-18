using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using System.IO;
using HamstarHelpers.Utilities.Config;
using HamstarHelpers.HudHelpers;
using Injury.NetProtocol;
using System;


namespace Injury {
	class InjuryMod : Mod {
		public static InjuryMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-injury-mod"; } }

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + InjuryConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			InjuryMod.Instance.Config.LoadFile();
		}


		////////////////

		public JsonConfig<InjuryConfigData> Config { get; private set; }

		public Texture2D HeartTex { get; private set; }


		////////////////

		public InjuryMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.Config = new JsonConfig<InjuryConfigData>( InjuryConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new InjuryConfigData() );
		}

		////////////////

		public override void Load() {
			InjuryMod.Instance = this;

			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_vers = new Version( 1, 2, 0 );
			if( hamhelpmod.Version < min_vers ) {
				throw new Exception( "Hamstar Helpers must be version " + min_vers.ToString() + " or greater." );
			}

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
				ErrorLogger.Log( "Injury updated to " + InjuryConfigData.ConfigVersion.ToString() );
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
