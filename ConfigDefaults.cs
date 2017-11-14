﻿using HamstarHelpers.Utilities.Config;
using System;


namespace Injury {
	public class InjuryConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 1, 9, 5 );
		public readonly static string ConfigFileName = "Injury Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public float PercentOfDamageToUseAsInjury = 0.075f;
		public float AdditionalInjuryPerDamagingHit = 0f;
		public float HarmBufferCapacityBeforeReceivingInjury = 5f;
		public int MaxHealthLostFromInjury = 5;

		public int FallLimpDurationMultiplier = 9;
		public float FallLimpSpeedMultiplier = 0.45f;
		public float FallLimpJumpMultiplier = 0.35f;

		public int LowestAllowedMaxHealth = 20;

		public float InjuryBufferHealPerSecond = 1f / (60f * 75f);  // 1 hp every 75 seconds
		public float BandOfLifeInjuryHealPerSecond = 1f / (60f * 30f); // 1 hp every 30 seconds

		public bool HighMaxHealthReducesInjury = true;

		public bool BrokenHeartsDrop = true;
		public int DurationOfBleedingHeart = 24 * 60;
		public int BrokenHeartsPerLifeCrystal = 4;
		public int BrokenHeartsPerCrackedLifeCrystal = 2;

		public bool CraftableBandOfLife = true;
		public bool CraftableLifeCrystal = true;
		public bool CraftableCrackedLifeCrystal = true;
		public bool LifeCrystalNeedsEvilBossDrops = true;

		public int TemporaryMaxHpChunkDrainTickRate = 5 * 30 * 60;   // 5 hp every 30 seconds

		public float MaxHpPercentRemainingUntilBleeding = 0.35f;
		public float MaxHpPercentLossForPowerfulBlowStagger = 0.25f;

		public float MaxHpPercentAsDamageAtFullHealthUntilHarm = 0.20f; // Adventurer's grace



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new InjuryConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= InjuryConfigData.ConfigVersion ) {
				return false;
			}
			
			if( vers_since < new Version( 1, 8, 1 ) ) {
				this.BandOfLifeInjuryHealPerSecond = new_config.BandOfLifeInjuryHealPerSecond;
			}
			if( vers_since < new Version( 1, 9, 2 ) ) {
				this.DurationOfBleedingHeart = new_config.DurationOfBleedingHeart;
			}

			this.VersionSinceUpdate = InjuryConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}