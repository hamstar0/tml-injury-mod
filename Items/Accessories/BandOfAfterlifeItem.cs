﻿using HamstarHelpers.Helpers.TModLoader;
using Injury.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items.Accessories {
	[AutoloadEquip( EquipType.HandsOn )]
	class BandOfAfterlifeItem : ModItem {
		public static int Width = 28;
		public static int Height = 22;

		
		////////////////
		
		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Band of Afterlife" );
			this.Tooltip.SetDefault( "Slowly fills maximum life (up to 500)"
				+ '\n' + "Regeneration stacks with Band of Life"
				+ '\n' + "Broken Hearts drop closer and last longer" );
		}

		public override void SetDefaults() {
			this.item.width = BandOfAfterlifeItem.Width;
			this.item.height = BandOfAfterlifeItem.Height;
			this.item.maxStack = 1;
			this.item.value = Item.buyPrice( 0, 50, 0, 0 );
			this.item.rare = 5;
			//this.item.handOnSlot = 2;
			this.item.accessory = true;
		}


		////////////////

		public override void UpdateAccessory( Player player, bool hideVisual ) {
			var mymod = (InjuryMod)this.mod;
			var modplayer = TmlHelpers.SafelyGetModPlayer<InjuryPlayer>( player );
			var itemInfo = this.item.GetGlobalItem<BandOfLifeItemInfo>();
			bool canHeal = player.statLifeMax < 500;

			HeartstringsItem.ApplyHeartstringEffect( player );

			if( modplayer.Logic.HiddenHarmBuffer == 0 && itemInfo.HealBuffer < 5f ) {
				itemInfo.HealBuffer += mymod.Config.BandOfAfterlifeInjuryHealPerSecond;
			}

			if( itemInfo.HealBuffer >= 5f && canHeal ) {
				player.statLifeMax += 5;
				itemInfo.HealBuffer -= 5f;

				Main.PlaySound( SoundID.Item4, player.position );
			}
		}


		public override void AddRecipes() {
			var recipe = new BandOfAfterlifeItemRecipe( this );
			recipe.AddRecipe();
		}
	}




	class BandOfAfterlifeItemRecipe : ModRecipe {
		public BandOfAfterlifeItemRecipe( BandOfAfterlifeItem myitem ) : base( myitem.mod ) {
			this.AddTile( TileID.WorkBenches );

			this.AddIngredient( ModContent.GetInstance<BandOfLifeItem>(), 1 );
			this.AddIngredient( ModContent.GetInstance<HeartstringsItem>(), 1 );
			this.AddIngredient( ModContent.GetInstance<EnrichedVitaeItem>(), 20 );

			this.SetResult( myitem );
		}

		public override bool RecipeAvailable() {
			var mymod = (InjuryMod)this.mod;
			return mymod.Config.Enabled && mymod.Config.CraftableBandOfAfterlife;
		}
	}
}
