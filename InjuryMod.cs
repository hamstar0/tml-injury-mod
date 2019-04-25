using Terraria.ModLoader;
using Terraria;
using System.IO;
using Injury.NetProtocol;
using System;
using Terraria.UI;
using System.Collections.Generic;
using Injury.Items.Consumables;
using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.TmlHelpers.ModHelpers;


namespace Injury {
	partial class InjuryMod : Mod {
		public static InjuryMod Instance { get; private set; }
		

		////////////////

		public JsonConfig<InjuryConfigData> ConfigJson { get; private set; }
		public InjuryConfigData Config { get { return this.ConfigJson.Data; } }

		public HealthLossDisplay HealthLoss { get; private set; }


		////////////////

		public InjuryMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			this.ConfigJson = new JsonConfig<InjuryConfigData>( InjuryConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new InjuryConfigData() );
		}

		////////////////

		public override void Load() {
			InjuryMod.Instance = this;

			this.HealthLoss = new HealthLossDisplay();

			this.LoadConfig();
		}

		private void LoadConfig() {
			if( !this.ConfigJson.LoadFile() ) {
				this.ConfigJson.SaveFile();
			}

			if( this.Config.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Injury updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}


		public override void Unload() {
			InjuryMod.Instance = null;
		}


		////////////////


		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( InjuryAPI ), args );
		}

		////////////////

		public override void HandlePacket( BinaryReader reader, int playerWho ) {
			if( Main.netMode == 1 ) {
				ClientPacketHandlers.RoutePacket( this, reader );
			} else if( Main.netMode == 2 ) {
				ServerPacketHandlers.RoutePacket( this, reader, playerWho );
			}
		}

		////////////////

		public override void AddRecipes() {
			var lifeCrystalRecipe = new LifeCrystalViaVitaeItemRecipe( this );
			lifeCrystalRecipe.AddRecipe();
		}


		////////////////

		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Resource Bars" ) );   //Vanilla: Inventory
			if( idx != -1 ) {
				GameInterfaceDrawMethod func = delegate {
					this.HealthLoss.DrawSubHealth( this, Main.spriteBatch );
					this.HealthLoss.DrawCurrentHeartDropAnimation( this, Main.spriteBatch );

					return true;
				};
				var interfaceLayer = new LegacyGameInterfaceLayer( "Injury: Heart Overlay", func, InterfaceScaleType.UI );

				layers.Insert( idx + 1, interfaceLayer );
			}
		}
	}
}
