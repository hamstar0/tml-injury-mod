using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Consumables {
	class LifeCrystalViaVitaeItemRecipe : ModRecipe {
		public LifeCrystalViaVitaeItemRecipe( InjuryMod mymod ) : base( mymod ) {
			this.AddTile( TileID.WorkBenches );

			if( mymod.Config.VitaePerLifeCrystal > 0 ) {
				this.AddIngredient( ModContent.GetInstance<VitaeItem>(), mymod.Config.VitaePerLifeCrystal );
			}
			if( mymod.Config.LifeCrystalNeedsEvilBossDrops ) {
				this.AddRecipeGroup( "ModHelpers:EvilBiomeBossDrops", 4 );
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
