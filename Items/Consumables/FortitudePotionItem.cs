using HamstarHelpers.RecipeHelpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Consumables {
	class FortitudePotionItem : ModItem {
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Fortitude Potion" );
			this.Tooltip.SetDefault( "Adds temporary resistance to injury" );
		}

		public override void SetDefaults() {
			this.item.UseSound = SoundID.Item3;
			this.item.useStyle = 2;
			this.item.useTurn = true;
			this.item.useAnimation = 17;
			this.item.useTime = 17;
			this.item.maxStack = 30;
			this.item.consumable = true;
			this.item.width = 14;
			this.item.height = 24;
			//item.potion = true;
			this.item.buffType = this.mod.BuffType( "FortifiedBuff" );
			this.item.buffTime = 30 * 60;
			this.item.value = 1000;
			this.item.rare = 1;
		}

		public override bool UseItem( Player player ) {
			if( player.itemAnimation > 0 && player.itemTime == 0 ) {
				player.itemTime = this.item.useTime;
				return true;
			}
			return base.UseItem( player );
		}

		public override void AddRecipes() {
			ModRecipe recipe = new FortitudePotionItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class FortitudePotionItemRecipe : ModRecipe {
		public FortitudePotionItemRecipe( FortitudePotionItem moditem ) : base( moditem.mod ) {
			this.AddTile( TileID.Bottles );

			this.AddIngredient( ItemID.BottledWater, 1 );
			this.AddIngredient( this.mod.GetItem<VitaeItem>(), 1 );
			this.AddIngredient( ItemID.Bone, 1 );
			this.AddRecipeGroup( RecipeHelpers.EvilBossDrops.Key );
			
			this.SetResult( moditem );
		}

		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableFortitudePotions;
		}
	}
}
