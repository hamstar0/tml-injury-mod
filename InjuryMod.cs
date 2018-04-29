using Terraria.ModLoader;
using Terraria;
using System.IO;
using HamstarHelpers.Utilities.Config;
using Injury.NetProtocol;
using System;
using Terraria.UI;
using System.Collections.Generic;
using Injury.Items.Consumables;


namespace Injury {
	class InjuryMod : Mod {
		public static InjuryMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-injury-mod"; } }

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + InjuryConfigMetaData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( !InjuryMod.Instance.ServerConfig.LoadFile() ) {
				InjuryMod.Instance.ServerConfig.SaveFile();
			}
			if( !InjuryMod.Instance.ClientConfig.LoadFile() ) {
				InjuryMod.Instance.ClientConfig.SaveFile();
			}
		}


		////////////////

		public InjuryServerConfigData ServerConfig { get; internal set; }
		public InjuryClientConfigData ClientConfig { get; internal set; }
		public HealthLossDisplay HealthLoss { get; private set; }


		////////////////

		public InjuryMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		////////////////

		public override void Load() {
			InjuryMod.Instance = this;

			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_vers = new Version( 1, 2, 2 );
			if( hamhelpmod.Version < min_vers ) {
				throw new Exception( "Hamstar Helpers must be version " + min_vers.ToString() + " or greater." );
			}

			this.HealthLoss = new HealthLossDisplay();
			
			if( this.ServerConfig.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Injury updated to " + InjuryConfigMetaData.ConfigVersion.ToString() );
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

		public override void AddRecipes() {
			var life_crystal_recipe = new LifeCrystalViaVitaeItemRecipe( this );
			life_crystal_recipe.AddRecipe();
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Resource Bars" ) );   //Vanilla: Inventory
			if( idx != -1 ) {
				GameInterfaceDrawMethod func = delegate {
					if( this.ClientConfig.RenderSubHealth ) {
						this.HealthLoss.DrawSubHealth( this, Main.spriteBatch );
					}
					if( this.ClientConfig.RenderHudHeartDrops ) {
						this.HealthLoss.DrawCurrentHeartDropAnimation( this, Main.spriteBatch );
					}

					return true;
				};
				var interface_layer = new LegacyGameInterfaceLayer( "Injury: Heart Overlay", func, InterfaceScaleType.UI );

				layers.Insert( idx + 1, interface_layer );
			}
		}
	}
}
