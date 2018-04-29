using System;
using System.ComponentModel;
using Terraria.ModLoader;


namespace Injury {
	public class InjuryConfigMetaData {
		public readonly static Version ConfigVersion = new Version( 2, 1, 0 );
		public readonly static string ConfigFileName = "Injury Config.json";
	}



	public class InjuryServerConfigData : ModConfig {
		public override MultiplayerSyncMode Mode {
			get { return MultiplayerSyncMode.ServerDictates; }
		}

		public override void PostAutoLoad() {
			var mymod = (InjuryMod)this.mod;
			mymod.ServerConfig = this;

			if( this.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Injury updated to " + InjuryConfigMetaData.ConfigVersion.ToString() );
			}
		}


		public void Load() {
			// TODO
		}
		public void Save() {
			// TODO
		}


		////////////////

		[Label( "Version since last update to config data" )]
		[DefaultValue( "" )]
		public string VersionSinceUpdate = "";


		[DefaultValue( true )]
		public bool Enabled = true;

		[Label( "Display debug information" )]
		[DefaultValue( false )]
		public bool DebugModeInfo;

		[Label( "Take injury on death" )]
		[DefaultValue( true )]
		public bool InjuryOnDeath = true;


		[Label( "Percent of damage to build up to each injury with (pre-injury)" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.075f )]
		public float PercentOfDamageToUseAsInjury = 0.075f;

		[Label( "Amount of build up until injury (pre-injury)" )]
		[DefaultValue( 5f )]
		public float HarmBufferCapacityBeforeReceivingInjury = 5f;

		[Label( "Additional injury per damaging hit" )]
		[DefaultValue( 0f )]
		public float AdditionalInjuryPerDamagingHit = 0f;

		[Label( "Max health lost from injury" )]
		[DefaultValue( 5 )]
		public int MaxHealthLostFromInjury = 5;


		[Label( "Lowest allowed max health" )]
		[DefaultValue( 20 )]
		public int LowestAllowedMaxHealth = 20;


		[Label( "Pre-injury 1/60th second auto-heal amount" )]
		[DefaultValue( 1f / ( 60f * 75f ) )]
		public float InjuryBufferHealPerSecond = 1f / ( 60f * 75f );  // 1 hp every 75 seconds
		[Label( "Pre-injury 1/60th second auto-heal amount via. Band of Life" )]
		[DefaultValue( 1f / ( 60f * 30f ) )]
		public float BandOfLifeInjuryHealPerSecond = 1f / ( 60f * 30f ); // 1 hp every 30 seconds
		[Label( "Pre-injury 1/60th second auto-heal amount via. Band of Afterlife" )]
		[DefaultValue( 1f / ( 60f * 45f ) )]
		public float BandOfAfterlifeInjuryHealPerSecond = 1f / ( 60f * 45f ); // 1 hp every 45 seconds


		[Label( "Reduce injury with high max health" )]
		[DefaultValue( true )]
		public bool HighMaxHealthReducesInjury = true;


		[Label( "Fall limp (1/60s units) duration multiplier-per-hp-lost" )]
		[DefaultValue( 9 )]
		public int FallLimpDurationMultiplier = 9;

		[Label( "Fall limp player speed multiplier" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.45f )]
		public float FallLimpSpeedMultiplier = 0.45f;

		[Label( "Fall limp player jump height multiplier" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.35f )]
		public float FallLimpJumpMultiplier = 0.35f;


		[Label( "Drop Broken Hearts on injury" )]
		[DefaultValue( true )]
		public bool BrokenHeartsDrop = true;

		[Label( "Duration of broken hearts (1/60s units)" )]
		[DefaultValue( 24 * 60 )]
		public int DurationOfBleedingHeart = 24 * 60;

		[Label( "Vitae required to craft Life Crystal" )]
		[DefaultValue( 4 )]
		public int VitaePerLifeCrystal = 4;

		[Label( "Vitae required to craft Cracked Life Crystal" )]
		[DefaultValue( 2 )]
		public int VitaePerCrackedLifeCrystal = 2;

		[Label( "Vitae required to craft Cracked Life Crystal" )]
		[DefaultValue( 5 )]
		public int EnrichedVitaeQuantityPerCraft = 3;

		[Label( "Evil boss drops needed per Life Crystal" )]
		[DefaultValue( true )]
		public bool LifeCrystalNeedsEvilBossDrops = true;


		[Label( "Craftable Band of Life" )]
		[DefaultValue( true )]
		public bool CraftableBandOfLife = true;

		[Label( "Craftable Vitae (beware curses!)" )]
		[DefaultValue( true )]
		public bool CraftableVitae = true;

		[Label( "Craftable Life Crystal" )]
		[DefaultValue( true )]
		public bool CraftableLifeCrystal = true;

		[Label( "Craftable Cracked Life Crystal" )]
		[DefaultValue( true )]
		public bool CraftableCrackedLifeCrystal = true;

		[Label( "Craftable Wandering Heart" )]
		[DefaultValue( true )]
		public bool CraftableWanderingHeart = true;

		[Label( "Craftable Heartstrings" )]
		[DefaultValue( true )]
		public bool CraftableHeartstrings = true;

		[Label( "Craftable Fortitude Potions" )]
		[DefaultValue( true )]
		public bool CraftableFortitudePotions = true;

		[Label( "Craftable Ambrosia" )]
		[DefaultValue( true )]
		public bool CraftableAmbrosia = true;

		[Label( "Craftable Band of Afterlife" )]
		[DefaultValue( true )]
		public bool CraftableBandOfAfterlife = true;

		[Label( "Craftable Band Vest" )]
		[DefaultValue( true )]
		public bool CraftableLifeVest = true;


		[Label( "Time (in 1/60s units) until loss of Cracked Life Crystal's temporary max hp" )]
		[DefaultValue( 5 * 30 * 60 )]
		public int TemporaryMaxHpChunkDrainTickRate = 5 * 30 * 60;   // 5 hp every 30 seconds


		[Label( "Life% remaining until Bleeding debuff appears" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.35f )]
		public float MaxHpPercentRemainingUntilBleeding = 0.35f;

		[Label( "Life% remaining before powerful blows cause staggering" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.25f )]
		public float MaxHpPercentLossForPowerfulBlowStagger = 0.25f;


		[Label( "Damage as max hp% before injury happens (when at full health; adventurer's grace)" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.20f )]
		public float MaxHpPercentAsDamageAtFullHealthUntilHarm = 0.20f; // Adventurer's grace


		[Label( "Broken Heart added duration via. Heartstrings (in 1/60s units)" )]
		[DefaultValue( 60 * 45 )]
		public int HeartstringsAddedDuration = 60 * 45; // +45s
		[Label( "Fortitude potion pre-injury capacity multiplier" )]
		[DefaultValue( 1.4f )]
		public float FortitudePotionHarmBufferMultiplier = 1.4f;
		[Label( "Life vest pre-injury capacity multiplier" )]
		[DefaultValue( 1.7f )]
		public float LifeVestHarmBufferMultiplier = 1.7f;


		[Label( "Odds of an accident while crafting Vitae" )]
		[DefaultValue( 6 )]
		public int VitaeCraftingAccidentOdds = 6;


		[Label( "Renders buffered damage before max hp loss" )]
		[DefaultValue( true )]
		public bool RenderSubHealth = true;

		[Label( "Renders life bar effect when max health is loss" )]
		[DefaultValue( true )]
		public bool RenderHudHeartDrops = true;
	}



	////////////////

	public class InjuryClientConfigData : ModConfig {
		public override MultiplayerSyncMode Mode {
			get { return MultiplayerSyncMode.UniquePerPlayer; }
		}

		public override void PostAutoLoad() {
			var mymod = (InjuryMod)this.mod;
			mymod.ClientConfig = this;

			if( this.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Injury updated to " + InjuryConfigMetaData.ConfigVersion.ToString() );
			}
		}


		public void Load() {
			// TODO
		}
		public void Save() {
			// TODO
		}


		////////////////

		[Label( "Version since last update to config data" )]
		[DefaultValue( "" )]
		public string VersionSinceUpdate = "";

		[Label( "Render sub health (how close to injury you are)" )]
		[DefaultValue( true )]
		public bool RenderSubHealth = true;

		[Label( "Render HUD injury heart drop animation" )]
		[DefaultValue( true )]
		public bool RenderHudHeartDrops = true;
	}
}
