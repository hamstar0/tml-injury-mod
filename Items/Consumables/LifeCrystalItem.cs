using HamstarHelpers.RecipeHelpers;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Consumables {
	class LifeCrystalViaVitaeItemRecipe : ModRecipe {
		static LifeCrystalViaVitaeItemRecipe() {
			var mymod = (InjuryMod)ModLoader.GetMod( "Injury" );

			mymod.AddRecipeEvt += delegate () {
				var myrecipe = new LifeCrystalViaVitaeItemRecipe( mymod );
				myrecipe.AddRecipe();
			};
		}



		public LifeCrystalViaVitaeItemRecipe( InjuryMod mymod ) : base( mymod ) {
			this.AddTile( TileID.WorkBenches ); 

			this.AddIngredient( mymod.GetItem<VitaeItem>(), mymod.Config.Data.VitaePerLifeCrystal );
			if( mymod.Config.Data.LifeCrystalNeedsEvilBossDrops ) {
				this.AddRecipeGroup( RecipeHelpers.EvilBossDrops.Key, 4 );
			}
			this.AddIngredient( ItemID.RubyGemsparkBlock, 10 );

			this.SetResult( ItemID.LifeCrystal, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableLifeCrystal;
		}
	}
}
