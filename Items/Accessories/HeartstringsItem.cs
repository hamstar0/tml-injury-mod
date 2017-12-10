using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Accessories {
	class HeartstringsItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;

		////////////////

		public static void ApplyHeartstringEffect( Player player ) {
			var modplayer = player.GetModPlayer<InjuryPlayer>();
			if( modplayer.HeartstringsEffectDuration == 0 ) {
				modplayer.HeartstringsEffectDuration = 2;
			} else if( modplayer.HeartstringsEffectDuration < 2 ) {
				modplayer.HeartstringsEffectDuration += 1;
			}
		}


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Heartstrings" );
			this.Tooltip.SetDefault( "Broken Hearts drop nearer and last longer" );
		}

		public override void SetDefaults() {
			this.item.width = HeartstringsItem.Width;
			this.item.height = HeartstringsItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 20, 0, 0 );
			this.item.rare = 4;
			this.item.accessory = true;
		}


		////////////////

		public override void UpdateAccessory( Player player, bool hide_visual ) {
			HeartstringsItem.ApplyHeartstringEffect( player );
		}

		////////////////

		public override void AddRecipes() {
			var recipe = new HeartstringsItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class HeartstringsItemRecipe : ModRecipe {
		public HeartstringsItemRecipe( HeartstringsItem myitem ) : base( myitem.mod ) {
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
