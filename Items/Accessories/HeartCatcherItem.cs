using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	class HeartCatcherItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;

		
		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Heartstrings" );
			this.Tooltip.SetDefault( "Broken Hearts last longer" );
		}

		public override void SetDefaults() {
			this.item.width = HeartCatcherItem.Width;
			this.item.height = HeartCatcherItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 1, 0, 0, 0 );
			this.item.rare = 5;
			//this.item.handOnSlot = 2;
			this.item.accessory = true;
		}


		////////////////

		public override void UpdateAccessory( Player player, bool hide_visual ) {
			var mymod = (InjuryMod)this.mod;
			var modplayer = player.GetModPlayer<InjuryPlayer>( this.mod );
			
			//TODO
		}


		public override void AddRecipes() {
			var recipe = new HeartCatcherItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class HeartCatcherItemRecipe : ModRecipe {
		public HeartCatcherItemRecipe( HeartCatcherItem myitem ) : base( myitem.mod ) {
			//this.AddTile( 114 );   // Tinkerer's Workshop
			this.AddTile( 18 );   // Crafting bench
			this.AddIngredient( ItemID.BandofRegeneration, 1 );
			this.AddIngredient( ItemID.LifeCrystal, 4 );
			this.AddIngredient( ItemID.PixieDust, 10 );
			this.AddIngredient( ItemID.RegenerationPotion, 10 );
			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableBandOfLife;
		}
	}
}
