using HamstarHelpers.ItemHelpers;
using Injury.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Injury.Projectiles {
	class BleedingHeartProjectile : ModProjectile {
		public static int GetDuration( InjuryMod mymod, InjuryPlayer modplayer ) {
			if( modplayer != null && modplayer.HeartstringsEffectDuration > 0 ) {
				return mymod.Config.DurationOfBleedingHeart + mymod.Config.HeartstringsAddedDuration;
			}
			return mymod.Config.DurationOfBleedingHeart;
		}

		////////////////

		public static void Spawn( Player player, InjuryMod mymod ) {
			var modplayer = player.GetModPlayer<InjuryPlayer>();
			int proj_type = mymod.ProjectileType<BleedingHeartProjectile>();
			float vel_x = 0, vel_y = 0;

			do {
				vel_x = ( Main.rand.NextFloat() * 10f ) - 5f;
				vel_y = ( Main.rand.NextFloat() * 7.5f ) - 5f;
			} while( Math.Abs( vel_x ) + Math.Abs( vel_y ) < 6f );

			if( modplayer.HeartstringsEffectDuration > 0 ) {
				vel_x /= 2f;
				vel_y /= 2f;
			}

			int proj_id = Projectile.NewProjectile( player.position.X, player.position.Y, vel_x, vel_y, proj_type, 0, 0, player.whoAmI, 0f, 0f );
			Projectile proj = Main.projectile[proj_id];

			proj.timeLeft = WanderingHeartProjectile.GetDuration( mymod, modplayer );
		}

		public static void GiveBrokenHeart( Player player, InjuryMod mymod ) {
			int item_which = ItemHelpers.CreateItem( player.Center, mymod.ItemType<BrokenHeartItem>(), 1, 16, 16 );
			Item item = Main.item[item_which];
			item.noGrabDelay = 3;
		}



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Bleeding Heart" );
		}

		public override void SetDefaults() {
			var mymod = (InjuryMod)this.mod;

			this.projectile.width = 16;
			this.projectile.height = 16;
			this.projectile.aiStyle = 14;
			this.projectile.penetrate = -1;
			this.projectile.netImportant = true;
			this.projectile.timeLeft = mymod.Config.DurationOfBleedingHeart;
		}

		////////////////

		public override void AI() {
			var mymod = (InjuryMod)this.mod;
			var proj = this.projectile;
			var myplayer = Main.player[proj.owner];
			InjuryPlayer modplayer = myplayer != null && myplayer.active ? myplayer.GetModPlayer<InjuryPlayer>() : null;

			int duration = BleedingHeartProjectile.GetDuration( mymod, modplayer );
			int proj_x = (int)proj.position.X - proj.width;
			int proj_y = (int)proj.position.Y - proj.height;
			int proj_width = proj.width * 3;
			int proj_height = proj.height * 3;
			var proj_rect = new Rectangle( proj_x, proj_y, proj_width, proj_height );

			// Spew particles
			if( ( proj.timeLeft > 60 && proj.timeLeft % 2 == 0 ) || proj.timeLeft % 5 == 0 ) {
				int blood_who = Dust.NewDust( proj.Center, 3, 6, 216, 0, 1f, 0, Color.Red, 1f );
				Main.dust[blood_who].velocity /= 2f;
				Main.dust[blood_who].scale = 0.8f;

				if( Main.rand.Next( 7 ) == 0 ) {
					int spark_who = Dust.NewDust( proj.position, proj.width, proj.height, 55, 0f, 0f, 200, Color.White, 1f );
					Main.dust[spark_who].velocity *= 0.1f;
					Main.dust[spark_who].scale *= 0.4f;
				}
			}

			// Enable item pickup
			if( proj.timeLeft < ( duration - 180 ) ) {
				for( int i = 0; i < 255; i++ ) {
					Player player = Main.player[i];
					if( player == null || !player.active || player.dead ) { continue; }

					Rectangle player_rect = new Rectangle( (int)player.position.X, (int)player.position.Y, player.width, player.height );

					if( proj_rect.Intersects( player_rect ) ) {
						BleedingHeartProjectile.GiveBrokenHeart( player, mymod );
						proj.Kill();
						break;
					}
				}
			}
		}
	}
}
