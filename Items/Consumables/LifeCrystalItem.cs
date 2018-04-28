using HamstarHelpers.RecipeHelpers;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Consumables {
	class LifeCrystalViaVitaeItemRecipe : ModRecipe {
		public LifeCrystalViaVitaeItemRecipe( InjuryMod mymod ) : base( mymod ) {
			this.AddTile( TileID.WorkBenches );

			this.AddIngredient( mymod.GetItem<VitaeItem>(), mymod.Config.VitaePerLifeCrystal );
			if( mymod.Config.LifeCrystalNeedsEvilBossDrops ) {
				this.AddRecipeGroup( RecipeHelpers.EvilBossDrops.Key, 4 );
			}
			this.AddIngredient( ItemID.RubyGemsparkBlock, 10 );

			this.SetResult( ItemID.LifeCrystal, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableLifeCrystal;
		}
	}
}
