﻿using Terraria;
using Terraria.ModLoader;


namespace Injury.Buffs {
	class ImpactTrauma : ModBuff {
		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Impact Trauma" );
			this.Description.SetDefault( "You've been hit hard by something" );

			Main.debuff[this.Type] = true;
			Main.pvpBuff[this.Type] = true;
		}

		public override void Update( Player player, ref int buffIndex ) {
			if( player.mount.Active ) { return; }

			MyPlayer info = player.GetModPlayer<MyPlayer>( this.mod );
			info.IsImpaired = true;
		}


		public static void ApplyImpairment( InjuryMod mymod, Player player ) {
			player.maxRunSpeed *= mymod.Config.Data.FallLimpSpeedMultiplier;
			player.accRunSpeed = player.maxRunSpeed;
			player.moveSpeed *= mymod.Config.Data.FallLimpSpeedMultiplier;

			int maxJump = (int)(Player.jumpHeight * mymod.Config.Data.FallLimpJumpMultiplier);
			if( player.jump > maxJump ) { player.jump = maxJump; }
		}
	}
}
