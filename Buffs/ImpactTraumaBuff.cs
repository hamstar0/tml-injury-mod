using HamstarHelpers.Helpers.TModLoader;
using Terraria;
using Terraria.ModLoader;


namespace Injury.Buffs {
	class ImpactTraumaBuff : ModBuff {
		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Impact Trauma" );
			this.Description.SetDefault( "You've been hit hard by something" );

			Main.debuff[this.Type] = true;
			Main.pvpBuff[this.Type] = true;
		}

		public override void Update( Player player, ref int buffIndex ) {
			if( player.mount.Active ) { return; }

			var myplayer = TmlHelpers.SafelyGetModPlayer<InjuryPlayer>( player );
			myplayer.IsImpaired = true;
		}


		public static void ApplyImpairment( InjuryMod mymod, Player player ) {
			player.maxRunSpeed *= mymod.Config.FallLimpSpeedMultiplier;
			player.accRunSpeed = player.maxRunSpeed;
			player.moveSpeed *= mymod.Config.FallLimpSpeedMultiplier;

			int maxJump = (int)(Player.jumpHeight * mymod.Config.FallLimpJumpMultiplier);
			if( player.jump > maxJump ) { player.jump = maxJump; }
		}
	}
}
