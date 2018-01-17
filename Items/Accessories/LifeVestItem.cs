using Injury.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Accessories {
	[AutoloadEquip( EquipType.Neck )]
	class LifeVestItem : ModItem {
		public static int Width = 18;
		public static int Height = 20;

		
		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Life Vest" );
			this.Tooltip.SetDefault( "Fortifies against injury"
				+ '\n' + "Also works as a floatation device" );
		}

		public override void SetDefaults() {
			this.item.width = LifeVestItem.Width;
			this.item.height = LifeVestItem.Height;
			this.item.maxStack = 1;
			this.item.defense = 4;
			this.item.value = Item.buyPrice( 0, 35, 0, 0 );
			this.item.rare = 5;
			this.item.accessory = true;
		}


		////////////////

		public override void UpdateAccessory( Player player, bool hide_visual ) {
			var mymod = (InjuryMod)this.mod;
			var modplayer = player.GetModPlayer<InjuryPlayer>();

			if( modplayer.LifeVestPresence == 0 ) { modplayer.LifeVestPresence = 2; }
			else { modplayer.LifeVestPresence++; }

			if( player.wet && player.velocity.Y > -1f ) {
				player.velocity.Y -= 0.24f;
			}
		}


		public override void AddRecipes() {
			var recipe = new LifeVestItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class LifeVestItemRecipe : ModRecipe {
		public LifeVestItemRecipe( LifeVestItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.TinkerersWorkbench );
			this.AddIngredient( ItemID.Silk, 10 );
			this.AddIngredient( ItemID.Bubble, 50 );
			this.AddIngredient( ItemID.ShroomiteBar, 5 );
			this.AddIngredient( this.mod.GetItem<EnrichedVitaeItem>(), 10 );
			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableLifeVest;
		}
	}
}
