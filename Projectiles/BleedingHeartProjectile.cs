using Injury.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Utils;


namespace Injury.Projectiles {
	public class BleedingHeartProjectile : ModProjectile {
		public static void Spawn( Player player, Mod mod ) {
			int proj_type = mod.ProjectileType<BleedingHeartProjectile>();
			float vel_x = 0, vel_y = 0;

			do {
				vel_x = (Main.rand.NextFloat() * 12f) - 6f;
				vel_y = (Main.rand.NextFloat() * 9f) - 6f;
			} while( Math.Abs(vel_x) + Math.Abs(vel_y) < 6f );

			Projectile.NewProjectile( player.position.X, player.position.Y, vel_x, vel_y, proj_type, 0, 0, Main.myPlayer, 0f, 0f );
		}

		public static void GiveBrokenHeart( Player player, Mod mod ) {
			int item_which = ItemHelper.CreateItem( player.Center, mod.ItemType<BrokenHeartItem>(), 1, 16, 16 );
			Item item = Main.item[item_which];
			item.noGrabDelay = 3;
		}



		public override void SetDefaults() {
			this.projectile.name = "Bleeding Heart";
			this.projectile.width = 16;
			this.projectile.height = 16;
			this.projectile.aiStyle = 14;
			this.projectile.penetrate = -1;
			this.projectile.netImportant = true;
			this.projectile.timeLeft = 600;
		}


		public override void AI() {
			if( (this.projectile.timeLeft > 60 && this.projectile.timeLeft % 2 == 0) || this.projectile.timeLeft % 5 == 0 ) {
				int dust_id = Dust.NewDust( this.projectile.Center, 3, 6, 216, 0, 1f, 0, Color.Red, 1f );
				Main.dust[dust_id].velocity /= 2f;
				Main.dust[dust_id].scale = 0.8f;
			}

			Rectangle proj_rect = new Rectangle( (int)this.projectile.position.X, (int)this.projectile.position.Y, this.projectile.width, this.projectile.height );
			
			for( int i = 0; i < 255; i++ ) {
				Player player = Main.player[i];
				if( player.active && !player.dead ) {
					if( this.projectile.timeLeft >= 570 && this.projectile.owner == player.whoAmI ) { continue; }

					Rectangle player_rect = new Rectangle( (int)player.position.X, (int)player.position.Y, player.width, player.height );

					if( proj_rect.Intersects( player_rect ) ) {
						BleedingHeartProjectile.GiveBrokenHeart( player, this.mod );
						this.projectile.Kill();
					}
				}
			}
		}
	}
}
