using HamstarHelpers.Helpers.Items;
using Injury.Items;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;


namespace Injury.Projectiles {
	class WanderingHeartProjectile : ModProjectile {
		public static int GetDuration( InjuryPlayer myplayer ) {
			var mymod = InjuryMod.Instance;

			if( myplayer != null && myplayer.HeartstringsEffectDuration > 0 ) {
				return mymod.Config.DurationOfBleedingHeart + mymod.Config.HeartstringsAddedDuration;
			}
			return mymod.Config.DurationOfBleedingHeart;
		}

		////////////////

		public static void Spawn( Player player ) {
			var mymod = InjuryMod.Instance;
			var modplayer = player.GetModPlayer<InjuryPlayer>();
			int projType = mymod.ProjectileType<WanderingHeartProjectile>();
			float velX = 0, velY = 0;

			do {
				velX = (Main.rand.NextFloat() * 20f) - 10f;
				velY = (Main.rand.NextFloat() * 15f) - 7.5f;
			} while( Math.Abs( velX ) + Math.Abs( velY ) < 8f );

			int projId = Projectile.NewProjectile( player.position.X, player.position.Y, velX, velY, projType, 0, 0, player.whoAmI, 0f, 0f );
			Projectile proj = Main.projectile[projId];

			proj.timeLeft = WanderingHeartProjectile.GetDuration( modplayer );
		}

		public static void GiveHeartItem( Player player, Mod mod ) {
			int itemWhich = ItemHelpers.CreateItem( player.Center, mod.ItemType<WanderingHeartItem>(), 1, 16, 16 );
			Item item = Main.item[ itemWhich ];
			item.noGrabDelay = 3;
		}



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Wandering Heart" );
		}

		public override void SetDefaults() {
			this.projectile.width = 16;
			this.projectile.height = 16;
			this.projectile.aiStyle = 14;
			this.projectile.penetrate = -1;
			this.projectile.netImportant = true;
			this.projectile.timeLeft = WanderingHeartProjectile.GetDuration( null );
		}

		////////////////

		public override void AI() {
			var mymod = (InjuryMod)this.mod;
			var proj = this.projectile;
			var myplayer = Main.player[proj.owner];
			InjuryPlayer modplayer = myplayer != null && myplayer.active ? myplayer.GetModPlayer<InjuryPlayer>() : null;

			int duration = WanderingHeartProjectile.GetDuration( modplayer );
			int projX = (int)proj.position.X - proj.width;
			int projY = (int)proj.position.Y - proj.height;
			int projWidth = proj.width * 3;
			int projHeight = proj.height * 3;
			var projRect = new Rectangle( projX - proj.width, projY - proj.height, projWidth, projHeight );

			// Preserve some bounciness
			if( proj.velocity.Y < 0 ) { proj.velocity.Y *= 1.02f; }

			if( (proj.timeLeft > 60 && proj.timeLeft % 2 == 0) || proj.timeLeft % 5 == 0 ) {
				if( Main.rand.Next( 4 ) == 0 ) {
					int sparkWho = Dust.NewDust( proj.position, proj.width, proj.height, 55, 0f, 0f, 200, Color.White, 1f );
					Main.dust[sparkWho].velocity *= 0.1f;
					Main.dust[sparkWho].scale *= 0.4f;
				}
			}
			
			if( proj.timeLeft < (duration - 180) ) {
				for( int i = 0; i < 255; i++ ) {
					Player player = Main.player[i];
					if( player == null || !player.active || player.dead ) { continue; }

					Rectangle playerRect = new Rectangle( (int)player.position.X, (int)player.position.Y, player.width, player.height );

					if( projRect.Intersects( playerRect ) ) {
						WanderingHeartProjectile.GiveHeartItem( player, this.mod );
						proj.Kill();
						break;
					}
				}
			}
		}
	}
}
