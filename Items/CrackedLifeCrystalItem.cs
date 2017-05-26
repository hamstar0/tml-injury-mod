using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	public class CrackedLifeCrystalItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;


		////////////////
		
		public override void SetDefaults() {
			this.item.name = "Cracked Life Crystal";
			this.item.toolTip = "Temporarily increases maximum life by 20";
			this.item.toolTip2 = "";
			this.item.width = CrackedLifeCrystalItem.Width;
			this.item.height = CrackedLifeCrystalItem.Height;
			this.item.consumable = true;
			this.item.useStyle = 4;
			this.item.useTime = 30;
			this.item.useAnimation = 30;
			this.item.UseSound = SoundID.Item4;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 2, 50, 0 );	// Sells for 50s
			this.item.rare = 2;
		}

		public override bool UseItem( Player player ) {
			if( player.itemAnimation > 0 && player.itemTime == 0 ) {
				player.itemTime = item.useTime;
				return true;
			}
			return base.UseItem( player );
		}

		public override bool ConsumeItem( Player player ) {
			var modplayer = player.GetModPlayer<InjuryPlayer>( this.mod );
			bool can_heal = modplayer.CanTemporaryInjuryHeal( 20 );
			if( can_heal ) {
				modplayer.TemporaryInjuryHeal( 20 );
			}
			return can_heal;
		}


		public override void AddRecipes() {
			var mymod = (InjuryMod)this.mod;
			var recipe = new ModRecipe( mymod );

			recipe.AddTile( 18 );   // Crafting bench
			recipe.AddIngredient( mymod.GetItem<BrokenHeartItem>(), mymod.Config.Data.BrokenHeartsPerCrackedLifeCrystal );
			recipe.AddIngredient( "Glass", 16 );
			recipe.AddIngredient( "Regeneration Potion", 4 );
			recipe.SetResult( this.item.type, 1 );
			recipe.AddRecipe();
		}
	}
}
