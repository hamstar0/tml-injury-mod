using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	[AutoloadEquip( EquipType.HandsOn )]
	class BandOfAfterlifeItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;

		
		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Band of Life" );
			this.Tooltip.SetDefault( "Slowly fills maximum life (up to 400)" );
		}

		public override void SetDefaults() {
			this.item.width = BandOfLifeItem.Width;
			this.item.height = BandOfLifeItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 25, 0, 0 );
			this.item.rare = 5;
			//this.item.handOnSlot = 2;
			this.item.accessory = true;
		}


		////////////////

		public override void UpdateAccessory( Player player, bool hide_visual ) {
			var mymod = (InjuryMod)this.mod;
			var modplayer = player.GetModPlayer<InjuryPlayer>( this.mod );
		}


		public override void AddRecipes() {
			var recipe = new BandOfAfterlifeItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class BandOfAfterlifeItemRecipe : ModRecipe {
		public BandOfAfterlifeItemRecipe( BandOfAfterlifeItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.WorkBenches );

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
