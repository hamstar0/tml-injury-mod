using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	class LifeCerealItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Cracked Life Crystal" );
			this.Tooltip.SetDefault( "Temporarily increases maximum life by 20" );
		}

		public override void SetDefaults() {
			this.item.width = LifeCerealItem.Width;
			this.item.height = LifeCerealItem.Height;
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
			var myrecipe = new LifeCerealItemRecipe( mymod, this );
			myrecipe.AddRecipe();
		}
	}


	
	class LifeCerealItemRecipe : ModRecipe {
		public LifeCerealItemRecipe( InjuryMod mymod, LifeCerealItem myitem ) : base( mymod ) {
			this.AddTile( 18 );   // Crafting bench
			this.AddIngredient( mymod.GetItem<BrokenHeartItem>(), mymod.Config.Data.BrokenHeartsPerCrackedLifeCrystal );
			this.AddIngredient( ItemID.Glass, 16 );
			this.AddIngredient( ItemID.RegenerationPotion, 4 );
			this.SetResult( myitem.item.type, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Data.Enabled && mymod.Config.Data.CraftableCrackedLifeCrystal;
		}
	}
}
