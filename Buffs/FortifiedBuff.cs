using Terraria;
using Terraria.ModLoader;


namespace Injury.Buffs {
	class FortifiedBuff : ModBuff {
		public override void SetDefaults() {
			this.DisplayName.SetDefault( "Fortified" );
			this.Description.SetDefault( "Reduces max health loss" );

			Main.debuff[this.Type] = false;
		}
	}
}
