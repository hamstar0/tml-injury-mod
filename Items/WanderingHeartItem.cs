using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	class WanderingHeartItem : ModItem {
		public static int Width = 16;
		public static int Height = 16;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Wandering Heart" );
			this.Tooltip.SetDefault( "Captured chunks of extra-lively life essence" );
		}

		public override void SetDefaults() {
			this.item.width = WanderingHeartItem.Width;
			this.item.height = WanderingHeartItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 5, 0 );  // Sells for 30s
			this.item.rare = 2;
		}


		public override void AddRecipes() {
			var myrecipe = new WanderingHeartViaLifeFruitItemRecipe( this );
			myrecipe.AddRecipe();
		}
	}



	class WanderingHeartViaLifeFruitItemRecipe : ModRecipe {
		public WanderingHeartViaLifeFruitItemRecipe( WanderingHeartItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.WorkBenches );
			
			this.AddIngredient( ItemID.LifeFruit, 1 );

			this.SetResult( myitem, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.ServerConfig.Enabled && mymod.ServerConfig.CraftableWanderingHeart;
		}
	}
}
