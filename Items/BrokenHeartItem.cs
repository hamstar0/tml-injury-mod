using Terraria;
using Terraria.ModLoader;


namespace Injury.Items {
	public class BrokenHeartItem : ModItem {
		public static int Width = 16;
		public static int Height = 16;


		////////////////
		
		public override void SetDefaults() {
			this.item.name = "Broken Heart";
			this.item.toolTip = "Salvaged remnants of life essence";
			this.item.toolTip2 = "";
			this.item.width = BandOfLifeItem.Width;
			this.item.height = BandOfLifeItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );	// Sells for 20s
			this.item.rare = 2;
		}


		public override void AddRecipes() {
			var mymod = (InjuryMod)this.mod;
			var my_recipe = new ModRecipe( mymod );

			if( mymod.Config.Data.CraftableLifeCrystal ) {
				my_recipe.AddTile( 18 );   // Crafting bench
				my_recipe.AddIngredient( this, mymod.Config.Data.BrokenHeartsPerLifeCrystal );
				if( mymod.Config.Data.LifeCrystalNeedsEvilBossDrops ) {
					my_recipe.AddRecipeGroup( "InjuryMod:EvilBiomeBossDrop", 4 );
					//recipe.AddIngredient( "Shadow Scale", 4 );
					//recipe.AddIngredient( "Tissue Sample", 4 );
				}
				my_recipe.AddIngredient( "Glass", 16 );
				my_recipe.AddIngredient( "Regeneration Potion", 4 );
				my_recipe.SetResult( 29, 1 );   // Life crystal
				my_recipe.AddRecipe();
			}
		}
	}
}
