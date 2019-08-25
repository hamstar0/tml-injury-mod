using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Consumables {
	class EnrichedVitaeItem : ModItem {
		public static int Width = 12;
		public static int Height = 24;



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Enriched Vitae" );
			this.Tooltip.SetDefault( "Enriched life extract. A drink to die for."
				+ '\n'+"Increases maximum life by 5 (up to 500)" );
		}

		public override void ModifyTooltips( List<TooltipLine> tooltips ) {
			TooltipLine tip = new TooltipLine( this.mod, "poison_warn", "Highly toxic!" );
			tip.overrideColor = Color.LimeGreen;
			tooltips.Add( tip );
		}

		public override void SetDefaults() {
			this.item.width = EnrichedVitaeItem.Width;
			this.item.height = EnrichedVitaeItem.Height;
			this.item.consumable = true;
			this.item.useTime = 17;
			this.item.useAnimation = 17;
			this.item.useTurn = true;
			this.item.useStyle = 2;
			this.item.UseSound = SoundID.Item3;
			this.item.maxStack = 30;
			this.item.value = Item.buyPrice( 0, 2, 50, 0 );	// Sells for 50s
			this.item.rare = 3;
		}

		////////////////

		public override bool UseItem( Player player ) {
			if( player.itemAnimation > 0 && player.itemTime == 0 ) {
				player.itemTime = item.useTime;
				return true;
			}
			return base.UseItem( player );
		}
		
		public override bool ConsumeItem( Player player ) {
			bool canConsume = player.statLifeMax <= 495;
			
			if( canConsume ) {
				int potSick = 60 * 30;
				int poisoned = 60 * 30;
				int venomed = 60 * 15;

				int potSickIdx = player.FindBuffIndex( BuffID.PotionSickness );
				int poisonedIdx = player.FindBuffIndex( BuffID.Poisoned );
				int venomedIdx = player.FindBuffIndex( BuffID.Venom );

				if( potSickIdx != -1 ) { potSick += player.buffTime[potSickIdx]; }
				if( poisonedIdx != -1 ) { poisoned += player.buffTime[poisonedIdx]; }
				if( venomedIdx != -1 ) { venomed += player.buffTime[venomedIdx]; }

				player.AddBuff( BuffID.PotionSickness, potSick );
				player.AddBuff( BuffID.Poisoned, poisoned );
				player.AddBuff( BuffID.Venom, venomed );

				player.statLifeMax += 5;
			}

			return canConsume;
		}

		////////////////

		public override void AddRecipes() {
			var mymod = (InjuryMod)this.mod;

			if( mymod.Config.EnrichedVitaeQuantityPerCraft > 0 ) {
				var myrecipe = new EnrichedVitaeItemRecipe( this );
				myrecipe.AddRecipe();
			}
		}
	}



	
	class EnrichedVitaeItemRecipe : ModRecipe {
		public EnrichedVitaeItemRecipe( EnrichedVitaeItem myitem ) : base( myitem.mod ) {
			var mymod = (InjuryMod)this.mod;

			this.AddTile( TileID.Bottles );

			this.AddIngredient( mymod.GetItem<WanderingHeartItem>(), 2 );
			this.AddIngredient( mymod.GetItem<VitaeItem>(), 2 );
			this.AddRecipeGroup( "ModHelpers:StrangePlants", 1 );

			this.SetResult( myitem, mymod.Config.EnrichedVitaeQuantityPerCraft );
		}


		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableAmbrosia;
		}
	}
}
