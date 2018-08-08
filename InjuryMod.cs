using Terraria.ModLoader;
using Terraria;
using System.IO;
using Injury.NetProtocol;
using System;
using Terraria.UI;
using System.Collections.Generic;
using Injury.Items.Consumables;
using HamstarHelpers.Components.Config;


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
				ErrorLogger.Log( "Injury updated to " + InjuryConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}
		}


		public override void Unload() {
			InjuryMod.Instance = null;
		}


		////////////////


		public override object Call( params object[] args ) {
			if( args.Length == 0 ) { throw new Exception( "Undefined call type." ); }

			string call_type = args[0] as string;
			if( args == null ) { throw new Exception( "Invalid call type." ); }

			var new_args = new object[args.Length - 1];
			Array.Copy( args, 1, new_args, 0, args.Length - 1 );

			return InjuryAPI.Call( call_type, new_args );
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
					this.HealthLoss.DrawSubHealth( this, Main.spriteBatch );
					this.HealthLoss.DrawCurrentHeartDropAnimation( this, Main.spriteBatch );

					return true;
				};
				var interface_layer = new LegacyGameInterfaceLayer( "Injury: Heart Overlay", func, InterfaceScaleType.UI );

				layers.Insert( idx + 1, interface_layer );
			}
		}
	}
}
