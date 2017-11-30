using Terraria;
using Terraria.ModLoader;


namespace Injury.Items.Consumables {
	class VitaeItem : ModItem {
		public static int Width = 18;
		public static int Height = 18;


		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Vitae" );
			this.Tooltip.SetDefault( "Revitalizing substance extracted from living things" );
		}

		public override void SetDefaults() {
			this.item.width = VitaeItem.Width;
			this.item.height = VitaeItem.Height;
			this.item.maxStack = 99;
			this.item.value = Item.buyPrice( 0, 1, 0, 0 );  // Sells for 20s
			this.item.rare = 2;
		}
	}
}
