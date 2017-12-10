using Terraria;
using Terraria.ModLoader;


namespace Injury.Items {
	class WanderingHeartItem : ModItem {
		public static int Width = 16;
		public static int Height = 16;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Wandering Heart" );
			this.Tooltip.SetDefault( "Captured chunks of extra-lively life essence" );
		}

		public override void SetDefaults() {
			this.item.width = WanderingHeartItem.Width;
			this.item.height = WanderingHeartItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 5, 0 );  // Sells for 30s
			this.item.rare = 2;
		}
	}
}
