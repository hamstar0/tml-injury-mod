﻿using Terraria;


namespace Injury.Logic {
	partial class InjuryLogic {
		public void UpdateTemporaryHealth( InjuryMod mymod, Player player ) {
			// Erode temporary max hp
			if( this.TemporaryMaxHp > 0 ) {
				if( this.TemporaryMaxHpTimer == 0 ) {
					this.TemporaryMaxHpTimer = mymod.Config.Data.TemporaryMaxHpChunkDrainTickRate;
					this.TemporaryMaxHp -= 5;

					if( player.statLifeMax > mymod.Config.Data.LowestAllowedMaxHealth ) {
						player.statLifeMax -= 5;
						this.InjuryVisualFX( mymod, player );
					} else {
						this.TemporaryMaxHpTimer = 0;
						this.TemporaryMaxHp = 0;
					}
				} else {
					this.TemporaryMaxHpTimer -= 1;
				}
			}
		}


		////////////////
		
		public bool CanTemporaryInjuryHeal( Player player, int amt ) {
			return player.statLifeMax < 400;
		}


		////////////////

		public bool TemporaryInjuryHeal( InjuryMod mymod, Player player, int amt ) {
			player.statLifeMax += amt;
			this.TemporaryMaxHp += amt;
			this.TemporaryMaxHpTimer = mymod.Config.Data.TemporaryMaxHpChunkDrainTickRate;

			return true;
		}
	}
}