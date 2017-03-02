using Terraria;
using Terraria.ModLoader;
using Injury;

namespace Injury.Buffs {
	public class ImpactTrauma : ModBuff {
		public override void SetDefaults() {
			Main.buffName[this.Type] = "Impact Trauma";
			Main.buffTip[this.Type] = "You've been hit hard by something";
			Main.debuff[this.Type] = true;
			Main.pvpBuff[this.Type] = true;
		}

		public override void Update( Player player, ref int buffIndex ) {
			if( player.mount.Active ) { return; }

			InjuryPlayer info = player.GetModPlayer<InjuryPlayer>( this.mod );
			info.IsImpaired = true;
		}


		public static void ApplyImpairment( Player player ) {
			player.maxRunSpeed *= InjuryMod.Config.Data.FallLimpSpeedMultiplier;
			player.accRunSpeed = player.maxRunSpeed;
			player.moveSpeed *= InjuryMod.Config.Data.FallLimpSpeedMultiplier;

			int maxJump = (int)(Player.jumpHeight * InjuryMod.Config.Data.FallLimpJumpMultiplier);
			if( player.jump > maxJump ) { player.jump = maxJump; }
		}
	}
}
