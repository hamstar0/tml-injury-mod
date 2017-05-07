using Terraria;


namespace Utils {
	public class PlayerHelper {
		public static bool HasUsedNurse( Player player ) {
			return Main.npcChatText == Lang.dialog( 227, false ) ||
					Main.npcChatText == Lang.dialog( 228, false ) ||
					Main.npcChatText == Lang.dialog( 229, false ) ||
					Main.npcChatText == Lang.dialog( 230, false );
		}

		public static int ComputeImpendingFallDamage( Player player ) {
			if( player.mount.CanFly ) {
				return 0;
			}
			if( player.mount.Cart && Minecart.OnTrack(player.position, player.width, player.height) ) {
				return 0;
			}
			if( player.mount.Type == 1 ) {
				return 0;
			}

			int safetyMin = 25 + player.extraFall;
			int damage = (int)(player.position.Y / 16f) - player.fallStart;

			if( player.stoned ) {
				return (int)(((float)damage * player.gravDir - 2f) * 20f);
			}

			if( (player.gravDir == 1f && damage > safetyMin) || (player.gravDir == -1f && damage < -safetyMin) ) {
				if( player.noFallDmg ) {
					return 0;
				}
				for( int n = 3; n < 10; n++ ) {
					if( player.armor[n].stack > 0 && player.armor[n].wingSlot > -1 ) {
						return 0;
					}
				}

				int finalDamage = (int)((float)damage * player.gravDir - (float)safetyMin) * 10;
				if( player.mount.Active ) {
					finalDamage = (int)((float)finalDamage * player.mount.FallDamage);
				}
				return finalDamage;
			}

			return 0;
		}

		public static bool CanPlayerJump( Player player ) {
			return (player.sliding ||
					player.velocity.Y == 0f ||
					(player.mount.Active && player.mount.Type == 3 && player.wetSlime > 0) ||
					player.jumpAgainCloud ||
					player.jumpAgainSandstorm ||
					player.jumpAgainBlizzard ||
					player.jumpAgainFart ||
					player.jumpAgainSail ||
					player.jumpAgainUnicorn ||
					(player.wet && player.accFlipper && (!player.mount.Active || !player.mount.Cart)))
				&& (player.releaseJump ||
					(player.autoJump && (player.velocity.Y == 0f || player.sliding)));
		}

		public static void GetTopHeartPosition( Player player, ref int x, ref int y ) {
			x = Main.screenWidth - 66;
			y = 60;

			if( player.statLifeMax2 < 400 && (player.statLifeMax2 / 20) % 10 != 0 ) {
				x -= (10 - ((player.statLifeMax2 / 20) % 10)) * 26;
			}
			if( player.statLifeMax2 / 20 <= 10 ) {
				y -= 32;
			}
		}
	}
}
