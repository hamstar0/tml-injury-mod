using HamstarHelpers.Helpers.Items;
using Injury.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Injury.Projectiles {
	class BleedingHeartProjectile : ModProjectile {
		public static int GetDuration( InjuryPlayer modplayer ) {
			var mymod = InjuryMod.Instance;

			if( modplayer != null && modplayer.HeartstringsEffectDuration > 0 ) {
				return mymod.Config.DurationOfBleedingHeart + mymod.Config.HeartstringsAddedDuration;
			}
			return mymod.Config.DurationOfBleedingHeart;
		}

		////////////////

		public static void Spawn( Player player ) {
			var mymod = InjuryMod.Instance;
			var modplayer = player.GetModPlayer<InjuryPlayer>();
			int projType = mymod.ProjectileType<BleedingHeartProjectile>();
			float velX = 0, velY = 0;

			do {
				velX = (Main.rand.NextFloat() * 10f) - 5f;
				velY = (Main.rand.NextFloat() * 7.5f) - 5f;
			} while( Math.Abs(velX) + Math.Abs(velY) < 6f );

			if( modplayer.HeartstringsEffectDuration > 0 ) {
				velX /= 2f;
				velY /= 2f;
			}

			int projId = Projectile.NewProjectile( player.position.X, player.position.Y, velX, velY, projType, 0, 0, player.whoAmI, 0f, 0f );
			Projectile proj = Main.projectile[ projId ];

			proj.timeLeft = WanderingHeartProjectile.GetDuration( modplayer );
		}

		public static void GiveBrokenHeart( Player player ) {
			var mymod = InjuryMod.Instance;
			int itemWhich = ItemHelpers.CreateItem( player.Center, mymod.ItemType<BrokenHeartItem>(), 1, 16, 16 );
			Item item = Main.item[itemWhich];
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
			var plr = Main.player[proj.owner];
			InjuryPlayer myplayer = plr != null && plr.active ? plr.GetModPlayer<InjuryPlayer>() : null;

			int duration = BleedingHeartProjectile.GetDuration( myplayer );
			int projX = (int)proj.position.X - proj.width;
			int projY = (int)proj.position.Y - proj.height;
			int projWidth = proj.width * 3;
			int projHeight = proj.height * 3;
			var projRect = new Rectangle( projX, projY, projWidth, projHeight );
			
			// Spew particles
			if( (proj.timeLeft > 60 && proj.timeLeft % 2 == 0) || proj.timeLeft % 5 == 0 ) {
				int bloodWho = Dust.NewDust( proj.Center, 3, 6, 216, 0, 1f, 0, Color.Red, 1f );
				Main.dust[bloodWho].velocity /= 2f;
				Main.dust[bloodWho].scale = 0.8f;

				if( Main.rand.Next(7) == 0 ) {
					int sparkWho = Dust.NewDust( proj.position, proj.width, proj.height, 55, 0f, 0f, 200, Color.White, 1f );
					Main.dust[sparkWho].velocity *= 0.1f;
					Main.dust[sparkWho].scale *= 0.4f;
				}
			}
			
			// Enable item pickup
			if( proj.timeLeft < (duration - 180) ) {
				for( int i = 0; i < 255; i++ ) {
					Player player = Main.player[i];
					if( player == null || !player.active || player.dead ) { continue; }

					Rectangle playerRect = new Rectangle( (int)player.position.X, (int)player.position.Y, player.width, player.height );
						
					if( projRect.Intersects( playerRect ) ) {
						BleedingHeartProjectile.GiveBrokenHeart( player );
						proj.Kill();
						break;
					}
				}
			}
		}
	}
}
