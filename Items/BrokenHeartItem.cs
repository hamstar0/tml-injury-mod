using Terraria;
using Terraria.ModLoader;

namespace Injury.Items {
	public class BrokenHeartItem : ModItem {
		public static int Width = 16;
		public static int Height = 16;


		////////////////
		
		public override void SetDefaults() {
			this.item.name = "Broken Heart";
			this.item.toolTip = "Salvaged remnants of life essence.";
			this.item.toolTip2 = "";
			this.item.width = BandOfLifeItem.Width;
			this.item.height = BandOfLifeItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 0, 50, 0 );
			this.item.rare = 2;
		}


		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe( this.mod );
			recipe.AddTile( 18 );   // Crafting bench
			recipe.AddIngredient( this, 4 );
			recipe.AddRecipeGroup( "InjuryMod:EvilBiomeBossDrop", 4 );
			//recipe.AddIngredient( "Shadow Scale", 4 );
			//recipe.AddIngredient( "Tissue Sample", 4 );
			recipe.AddIngredient( "Glass", 32 );
			recipe.AddIngredient( "Regeneration Potion", 7 );
			recipe.SetResult( 29, 1 );	// Life crystal
			recipe.AddRecipe();
		}
	}
}
