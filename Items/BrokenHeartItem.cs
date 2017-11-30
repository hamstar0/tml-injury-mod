using HamstarHelpers.RecipeHelpers;
using Injury.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	class BrokenHeartItem : ModItem {
		public static int Width = 16;
		public static int Height = 16;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Broken Heart" );
			this.Tooltip.SetDefault( "Salvaged remnants of life essence" );
		}

		public override void SetDefaults() {
			this.item.width = BandOfLifeItem.Width;
			this.item.height = BandOfLifeItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );	// Sells for 20s
			this.item.rare = 2;
		}


		public override void AddRecipes() {
			var myrecipe = new VitaeViaBrokenHeartItemRecipe( this );
			myrecipe.AddRecipe();
		}
	}



	class VitaeViaBrokenHeartItemRecipe : ModRecipe {
		public VitaeViaBrokenHeartItemRecipe( BrokenHeartItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.DemonAltar );

			this.AddIngredient( myitem, 1 );
			this.AddRecipeGroup( RecipeHelpers.VanillaAnimals.Key, 1 );
			this.AddIngredient( ItemID.Mushroom, 1 );

			this.SetResult( this.mod.GetItem<VitaeItem>(), 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableLifeCrystal;
		}
	}
}
