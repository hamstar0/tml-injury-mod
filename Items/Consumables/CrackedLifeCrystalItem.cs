using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Consumables {
	class CrackedLifeCrystalItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Cracked Life Crystal" );
			this.Tooltip.SetDefault( "Temporarily increases maximum life by 20" );
		}

		public override void SetDefaults() {
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
			var myplayer = player.GetModPlayer<InjuryPlayer>();
			bool canHeal = myplayer.Logic.CanTemporaryInjuryHeal( player, 20 );

			if( canHeal ) {
				myplayer.Logic.TemporaryInjuryHeal( player, 20 );
			}

			return canHeal;
		}


		public override void AddRecipes() {
			var myrecipe = new CrackedLifeCrystalItemRecipe( this );
			myrecipe.AddRecipe();
		}
	}



	
	class CrackedLifeCrystalItemRecipe : ModRecipe {
		public CrackedLifeCrystalItemRecipe( CrackedLifeCrystalItem myitem ) : base( myitem.mod ) {
			var mymod = (InjuryMod)this.mod;

			this.AddTile( TileID.WorkBenches );

			this.AddIngredient( mymod.GetItem<VitaeItem>(), mymod.Config.VitaePerCrackedLifeCrystal );
			this.AddIngredient( ItemID.RubyGemsparkBlock, 10 );

			this.SetResult( myitem, 1 );
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableCrackedLifeCrystal;
		}
	}
}
