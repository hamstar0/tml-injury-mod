using HamstarHelpers.Classes.UI.ModConfig;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace Injury {
	class MyFloatInputElement : FloatInputElement { }





	public class InjuryConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;


		////

		[DefaultValue( true )]
		public bool Enabled = true;


		public bool DebugModeInfo = false;


		[DefaultValue( true )]
		public bool InjuryOnDeath = true;


		[Range( 0f, 1f )]
		[DefaultValue( 0.075f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float PercentOfDamageToUseAsInjury = 0.075f;

		[Range( 0f, 1000f )]
		[DefaultValue( 0f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float AdditionalInjuryPerDamagingHit = 0f;

		[Range( 0f, 1000f )]
		[DefaultValue( 5f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float HarmBufferCapacityBeforeReceivingInjury = 5f;

		[Range( 0, 1000 )]
		[DefaultValue( 5 )]
		public int MaxHealthLostFromInjury = 5;


		[Range( 0f, 100f )]
		[DefaultValue( 9f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float FallLimpDurationMultiplier = 9f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.45f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float FallLimpSpeedMultiplier = 0.45f;

		[Range( 0f, 100f )]
		[DefaultValue( 0.35f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float FallLimpJumpMultiplier = 0.35f;


		[Range( 1, 100 )]
		[DefaultValue( 20 )]
		public int LowestAllowedMaxHealth = 20;


		[Range( 0f, 100f )]
		[DefaultValue( 1f / ( 60f * 75f ) )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float InjuryBufferHealPerSecond = 1f / (60f * 75f);  // 1 hp every 75 seconds

		[Range( 0f, 100f )]
		[DefaultValue( 1f / ( 60f * 30f ) )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BandOfLifeInjuryHealPerSecond = 1f / (60f * 30f); // 1 hp every 30 seconds

		[Range( 0f, 100f )]
		[DefaultValue( 1f / ( 60f * 45f ) )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float BandOfAfterlifeInjuryHealPerSecond = 1f / (60f * 45f); // 1 hp every 45 seconds


		[DefaultValue( true )]
		public bool HighMaxHealthReducesInjury = true;


		[DefaultValue( true )]
		public bool BrokenHeartsDrop = true;

		[ReloadRequired]
		[Range( 1, 60 * 60 * 60 * 24 )]
		[DefaultValue( 24 * 60 )]
		public int DurationOfBleedingHeart = 24 * 60;

		[ReloadRequired]
		[Range( 0, 999 )]
		[DefaultValue( 4 )]
		public int VitaePerLifeCrystal = 4;

		[ReloadRequired]
		[Range( 0, 999 )]
		[DefaultValue( 2 )]
		public int VitaePerCrackedLifeCrystal = 2;

		[ReloadRequired]
		[Range( 0, 999 )]
		[DefaultValue( 3 )]
		public int EnrichedVitaeQuantityPerCraft = 3;


		[DefaultValue( true )]
		public bool CraftableBandOfLife = true;

		[DefaultValue( true )]
		public bool CraftableVitae = true;

		[DefaultValue( true )]
		public bool CraftableLifeCrystal = true;

		[DefaultValue( true )]
		 public bool LifeCrystalNeedsEvilBossDrops = true;

		[DefaultValue( true )]
		public bool CraftableCrackedLifeCrystal = true;

		[DefaultValue( true )]
		public bool CraftableWanderingHeart = true;

		[DefaultValue( true )]
		public bool CraftableHeartstrings = true;

		[DefaultValue( true )]
		public bool CraftableFortitudePotions = true;

		[DefaultValue( true )]
		public bool CraftableAmbrosia = true;

		[DefaultValue( true )]
		public bool CraftableBandOfAfterlife = true;

		[DefaultValue( true )]
		public bool CraftableLifeVest = true;


		[Range( 0, 60 * 60 * 60 * 24 )]
		[DefaultValue( 5 * 30 * 60 )]
		public int TemporaryMaxHpChunkDrainTickRate = 5 * 30 * 60;   // 1 hp every 2.5 minutes


		[Range( 0f, 1f )]
		[DefaultValue( 0.35f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float MaxHpPercentRemainingUntilBleeding = 0.35f;

		[Range( 0f, 1f )]
		[DefaultValue( 0.25f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float MaxHpPercentLossForPowerfulBlowStagger = 0.25f;


		[Range( 0f, 1f )]
		[DefaultValue( 0.20f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float MaxHpPercentAsDamageAtFullHealthUntilHarm = 0.20f; // "Adventurer's grace"


		[DefaultValue( true )]
		public bool RenderSubHealth = true;

		[DefaultValue( true )]
		public bool RenderHudHeartDrops = true;


		[Range( 0, 60 * 60 * 60 * 24 )]
		[DefaultValue( 60 * 45 )]
		public int HeartstringsAddedDuration = 60 * 45; // +45s

		[Range( 1f, 100f )]
		[DefaultValue( 1.4f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float FortitudePotionHarmAddedBufferMultiplier = 1.4f;

		[Range( 1f, 100f )]
		[DefaultValue( 1.7f )]
		[CustomModConfigItem( typeof( MyFloatInputElement ) )]
		public float LifeVestHarmAddedBufferMultiplier = 1.7f;


		[Range( 0, 1000 )]
		[DefaultValue( 6 )]
		public int VitaeCraftingAccidentOdds = 6;
	}
}
