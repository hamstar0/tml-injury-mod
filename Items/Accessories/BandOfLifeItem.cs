using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace Injury.Items {
	[AutoloadEquip( EquipType.HandsOn )]
	class BandOfLifeItem : ModItem {
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
			var item_info = this.item.GetGlobalItem<BandOfLifeItemInfo>( this.mod );

			if( modplayer.Logic.HiddenHarmBuffer == 0 && item_info.HealBuffer < 5f ) {
				item_info.HealBuffer += mymod.Config.Data.BandOfLifeInjuryHealPerSecond;
			}

			if( item_info.HealBuffer >= 5f && player.statLifeMax < 400 ) {
				player.statLifeMax += 5;
				item_info.HealBuffer -= 5f;

				Main.PlaySound( SoundID.Item4, player.position );
			}
		}


		public override void AddRecipes() {
			var recipe = new BandOfLifeItemRecipe( this );
			recipe.AddRecipe();
		}
	}



	class BandOfLifeItemRecipe : ModRecipe {
		public BandOfLifeItemRecipe( BandOfLifeItem myitem ) : base( myitem.mod ) {
			//this.AddTile( 114 );   // Tinkerer's Workshop
			this.AddTile( 18 );   // Crafting bench
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



	class BandOfLifeItemInfo : GlobalItem {
		public override bool InstancePerEntity { get { return true; } }
		//public override bool CloneNewInstances { get { return true; } }

		public float HealBuffer = 0;

		//public override GlobalItem NewInstance( Item item ) {
		//	return new BandOfLifeItemInfo();
		//}
		
		public override GlobalItem Clone( Item item, Item item_clone ) {
			var clone = (BandOfLifeItemInfo)base.Clone( item, item_clone );
			clone.HealBuffer = this.HealBuffer;
			return clone;
		}

		public override void NetSend( Item item, BinaryWriter writer ) {
			writer.Write( (float)this.HealBuffer );
		}

		public override void NetReceive( Item item, BinaryReader reader ) {
			this.HealBuffer = reader.ReadSingle();
		}
	}
}
