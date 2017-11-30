using Terraria;
using Terraria.ModLoader;


namespace Injury.Items {
	class StrayHeartItem : ModItem {
		public static int Width = 16;
		public static int Height = 16;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Broken Heart" );
			this.Tooltip.SetDefault( "Salvaged remnants of life essence" );
		}

		public override void SetDefaults() {
			this.item.width = BandOfLifeItem.Width;
			this.item.height = BandOfLifeItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );	// Sells for 20s
			this.item.rare = 2;
		}
	}
}
