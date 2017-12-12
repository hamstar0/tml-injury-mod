﻿using HamstarHelpers.RecipeHelpers;
using Injury.NetProtocol;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	class VitaeItem : ModItem {
		public static int Width = 18;
		public static int Height = 18;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Vitae" );
			this.Tooltip.SetDefault( "Life extract. Painfully extracted." );
		}

		public override void SetDefaults() {
			this.item.width = VitaeItem.Width;
			this.item.height = VitaeItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );  // Sells for 20s
			this.item.rare = 2;
		}


		public override void AddRecipes() {
			var myrecipe = new VitaeViaBrokenHeartItemRecipe( this );
			myrecipe.AddRecipe();
		}
	}



	class VitaeViaBrokenHeartItemRecipe : ModRecipe {
		public VitaeViaBrokenHeartItemRecipe( VitaeItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.DemonAltar );

			this.AddIngredient( this.mod.GetItem<BrokenHeartItem>(), 1 );
			this.AddRecipeGroup( RecipeHelpers.VanillaAnimals.Key, 1 );
			this.AddIngredient( ItemID.Mushroom, 1 );

			this.SetResult( myitem, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableLifeCrystal;
		}


		public override void OnCraft( Item item ) {
			var mymod = InjuryMod.Instance;
			int odds = mymod.Config.Data.VitaeCraftingAccidentOdds;

			if( !Main.hardMode && Main.rand.Next( odds ) == 0 ) {
				Main.LocalPlayer.AddBuff( BuffID.Cursed, 60 * 15 ); // 15 seconds of curse

				if( Main.netMode == 1 ) {
					ClientPacketHandlers.SendSpawnRequestFromClient( mymod, NPCID.Wraith );
				} else if( Main.netMode == 0 ) {
					NPC.SpawnOnPlayer( Main.LocalPlayer.whoAmI, NPCID.Wraith );
				}
				
				if( Main.LocalPlayer.FindBuffIndex(BuffID.Cursed) != -1 ) {
					Main.NewText( "An otherworldly presense has noticed your act. Your body seizes up.", Color.Red );
				} else {
					Main.NewText( "An otherworldly presense has noticed your act...", Color.Red );
				}
			}
		}
	}
}