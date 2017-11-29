using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	class HeartstringsItem : ModItem {
		public static int Width = 28;
		public static int Height = 30;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Heartstrings" );
			this.Tooltip.SetDefault( "Broken Hearts last longer before dissintegrating" );
		}

		public override void SetDefaults() {
			this.item.width = HeartstringsItem.Width;
			this.item.height = HeartstringsItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 2, 0, 0, 0 );
			this.item.rare = 5;
			this.item.accessory = true;
		}


		////////////////

		public override void UpdateAccessory( Player player, bool hide_visual ) {
			var modplayer = player.GetModPlayer<InjuryPlayer>();
			if( modplayer.HeartstringsEffectDuration == 0 ) {
				modplayer.HeartstringsEffectDuration = 2;
			} else {
				modplayer.HeartstringsEffectDuration += 1;
			}
		}

		////////////////

		public override void AddRecipes() {
			var recipe = new HeartstringsItemRecipe( (InjuryMod)this.mod, this );
			recipe.AddRecipe();
		}
	}



	class HeartstringsItemRecipe : ModRecipe {
		public HeartstringsItemRecipe( InjuryMod mymod, HeartstringsItem myitem ) : base( mymod ) {
			this.AddTile( TileID.TinkerersWorkbench );
			this.AddIngredient( ItemID.BeeWax, 20 );
			this.AddRecipeGroup( "HamstarHelpers:EvilBiomeLightPet", 1 );
			this.AddIngredient( ItemID.RainbowString, 1 );
			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableHeartstrings;
		}
	}
}
