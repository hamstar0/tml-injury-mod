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
			var mymod = (InjuryMod)this.mod;
			var myrecipe = new LifeCrystalViaBrokenHeartItemRecipe( mymod, this );
			myrecipe.AddRecipe();
		}
	}



	class LifeCrystalViaBrokenHeartItemRecipe : ModRecipe {
		public LifeCrystalViaBrokenHeartItemRecipe( InjuryMod mymod, BrokenHeartItem myitem ) : base( mymod ) {
			this.AddTile( 18 );   // Crafting bench
			this.AddIngredient( myitem, mymod.Config.Data.BrokenHeartsPerLifeCrystal );
			if( mymod.Config.Data.LifeCrystalNeedsEvilBossDrops ) {
				this.AddRecipeGroup( "InjuryMod:EvilBiomeBossDrop", 4 );
				//this.AddIngredient( "Shadow Scale", 4 );
				//this.AddIngredient( "Tissue Sample", 4 );
			}
			this.AddIngredient( ItemID.Glass, 16 );
			this.AddIngredient( ItemID.RegenerationPotion, 4 );
			this.SetResult( 29, 1 );   // Life crystal
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableLifeCrystal;
		}
	}
}
