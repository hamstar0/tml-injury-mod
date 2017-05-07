using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	public class BandOfLifeItem : ModItem {
		public static int Width = 22;
		public static int Height = 22;

		
		////////////////

		public override bool Autoload( ref string name, ref string texture, IList<EquipType> equips ) {
			equips.Add( EquipType.HandsOn );
			return true;
		}

		public override void SetDefaults() {
			this.item.name = "Band of Life";
			this.item.toolTip = "Slowly fills maximum life (up to 400)";
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
			var item_info = this.item.GetModInfo<BandOfLifeItemInfo>( this.mod );

			if( modplayer.HiddenHarmBuffer == 0 && item_info.HealBuffer < 5f ) {
				item_info.HealBuffer += mymod.Config.Data.BandOfLifeInjuryHealPerSecond;
			}

			if( item_info.HealBuffer >= 5f && player.statLifeMax < 400 ) {
				player.statLifeMax += 5;
				item_info.HealBuffer -= 5f;

				Main.PlaySound( SoundID.Item4, player.position );
			}
		}


		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe( this.mod );
			//recipe.AddTile( 114 );   // Tinkerer's Workshop
			recipe.AddTile( 18 );   // Crafting bench
			recipe.AddIngredient( "Band of Regeneration", 1 );
			recipe.AddIngredient( "Life Crystal", 4 );
			recipe.AddIngredient( "Pixie Dust", 10 );
			recipe.AddIngredient( "Regeneration Potion", 10 );
			recipe.SetResult( this );
			recipe.AddRecipe();
		}
	}



	class BandOfLifeItemInfo : ItemInfo {
		public float HealBuffer = 0;

		public override ItemInfo Clone() {
			var clone = base.Clone();
			var myclone = (BandOfLifeItemInfo)clone;
			myclone.HealBuffer = this.HealBuffer;
			return myclone;
		}
	}
}
