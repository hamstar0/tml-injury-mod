using Terraria.ModLoader;
using Terraria;
using System.IO;
using Injury.NetProtocol;
using System;
using Terraria.UI;
using System.Collections.Generic;
using Injury.Items.Consumables;
using HamstarHelpers.Helpers.TModLoader.Mods;


namespace Injury {
	public partial class InjuryMod : Mod {
		public static InjuryMod Instance { get; private set; }


		////////////////

		public InjuryConfig Config => ModContent.GetInstance<InjuryConfig>();

		internal HealthLossDisplay HealthLoss { get; private set; }


		////////////////

		public InjuryMod() {
			InjuryMod.Instance = this;
		}

		////////////////

		public override void Load() {
			this.HealthLoss = new HealthLossDisplay();
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
