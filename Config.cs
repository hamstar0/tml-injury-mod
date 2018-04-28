using HamstarHelpers.Utilities.Config;
using System;


namespace Injury {
	public class InjuryConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 2, 0, 2 );
		public readonly static string ConfigFileName = "Injury Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;

		public bool DebugModeInfo = false;

		public bool InjuryOnDeath = true;

		public float PercentOfDamageToUseAsInjury = 0.075f;
		public float AdditionalInjuryPerDamagingHit = 0f;
		public float HarmBufferCapacityBeforeReceivingInjury = 5f;
		public int MaxHealthLostFromInjury = 5;

		public int FallLimpDurationMultiplier = 9;
		public float FallLimpSpeedMultiplier = 0.45f;
		public float FallLimpJumpMultiplier = 0.35f;

		public int LowestAllowedMaxHealth = 20;

		public float InjuryBufferHealPerSecond = 1f / ( 60f * 75f );  // 1 hp every 75 seconds
		public float BandOfLifeInjuryHealPerSecond = 1f / ( 60f * 30f ); // 1 hp every 30 seconds
		public float BandOfAfterlifeInjuryHealPerSecond = 1f / ( 60f * 45f ); // 1 hp every 45 seconds

		public bool HighMaxHealthReducesInjury = true;

		public bool BrokenHeartsDrop = true;
		public int DurationOfBleedingHeart = 24 * 60;
		public int VitaePerLifeCrystal = 4;
		public int VitaePerCrackedLifeCrystal = 2;
		public int EnrichedVitaeQuantityPerCraft = 3;


		public bool CraftableBandOfLife = true;
		public bool CraftableVitae = true;
		public bool CraftableLifeCrystal = true;
		public bool LifeCrystalNeedsEvilBossDrops = true;
		public bool CraftableCrackedLifeCrystal = true;
		public bool CraftableWanderingHeart = true;
		public bool CraftableHeartstrings = true;
		public bool CraftableFortitudePotions = true;
		public bool CraftableAmbrosia = true;
		public bool CraftableBandOfAfterlife = true;
		public bool CraftableLifeVest = true;

		public int TemporaryMaxHpChunkDrainTickRate = 5 * 30 * 60;   // 5 hp every 30 seconds

		public float MaxHpPercentRemainingUntilBleeding = 0.35f;
		public float MaxHpPercentLossForPowerfulBlowStagger = 0.25f;

		public float MaxHpPercentAsDamageAtFullHealthUntilHarm = 0.20f; // Adventurer's grace

		public bool RenderSubHealth = true;
		public bool RenderHudHeartDrops = true;

		public int HeartstringsAddedDuration = 60 * 45; // +45s
		public float FortitudePotionHarmBufferMultiplier = 1.4f;
		public float LifeVestHarmBufferMultiplier = 1.7f;

		public int VitaeCraftingAccidentOdds = 6;


		////

		public string _OLD_SETTINGS_BELOW_ = "";

		public int BrokenHeartsPerLifeCrystal = 4;
		public int BrokenHeartsPerCrackedLifeCrystal = 2;



		////////////////

		public static int _2_0_0_EnrichedVitaeQuantityPerCraft = 3;
		public static int _2_0_2_EnrichedVitaeQuantityPerCraft = 5;


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
			if( vers_since < new Version( 2, 0, 2 ) ) {
				if( this.EnrichedVitaeQuantityPerCraft == InjuryConfigData._2_0_0_EnrichedVitaeQuantityPerCraft ) {
					this.EnrichedVitaeQuantityPerCraft = new_config.EnrichedVitaeQuantityPerCraft;
				}
			}
			if( vers_since < new Version( 2, 0, 3 ) ) {
				if( this.EnrichedVitaeQuantityPerCraft == InjuryConfigData._2_0_2_EnrichedVitaeQuantityPerCraft ) {
					this.EnrichedVitaeQuantityPerCraft = new_config.EnrichedVitaeQuantityPerCraft;
				}
			}

			this.VersionSinceUpdate = InjuryConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
