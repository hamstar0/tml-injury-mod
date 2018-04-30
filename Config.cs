using System;
using System.ComponentModel;
using Terraria.ModLoader;


namespace Injury {
	public class InjuryConfigMetaData {
		public readonly static Version ConfigVersion = new Version( 3, 0, 0 );
		public readonly static string ConfigFileName = "Injury Config.json";
	}



	[Label( "Client Settings" )]
	public class InjuryClientConfigData : ModConfig {
		public override MultiplayerSyncMode Mode {
			get { return MultiplayerSyncMode.UniquePerPlayer; }
		}

		public override void PostAutoLoad() {
			var mymod = (InjuryMod)this.mod;
			mymod.ClientConfig = this;
		}


		////////////////

		[Label( "Version since last update to config data" )]
		[DefaultValue( "" )]
		public string VersionSinceUpdate = "";

		[Tooltip( "Shown as a growing shadow upon your last life heart." )]
		[Label( "Renders injury buffer %" )]
		[DefaultValue( true )]
		public bool RenderInjuryBuffer = true;

		[Tooltip( "Shown as a heart dropping from where your last life heart was." )]
		[Label( "Renders max hp loss" )]
		[DefaultValue( true )]
		public bool RenderHudHeartDrops = true;
	}




	[Label( "Game Settings" )]
	public class InjuryServerConfigData : ModConfig {
		public override MultiplayerSyncMode Mode {
			get { return MultiplayerSyncMode.ServerDictates; }
		}

		public override void PostAutoLoad() {
			var mymod = (InjuryMod)this.mod;
			mymod.ServerConfig = this;
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


		[Tooltip( "Enables functions of the mod. Disable to use like an API." )]
		[DefaultValue( true )]
		public bool Enabled = true;

		[Label( "Display debug information" )]
		[DefaultValue( false )]
		public bool DebugModeInfo;


		[Label( "Take injury on death" )]
		[DefaultValue( true )]
		public bool InjuryOnDeath = true;


		[Tooltip( "As % of damage." )]
		[Label( "Damage to add to injury buffer" )]
		[Range( 0f, 1f )]
		[DefaultValue( 0.075f )]
		public float PercentOfDamageToUseAsInjury = 0.075f;

		[Label( "Injury buffer size" )]
		[Range( 1f, 100f )]
		[DefaultValue( 5f )]
		public float InjuryBufferSize = 5f;

		[Label( "Additional injury per damaging hit" )]
		[Range( 0f, 100f )]
		[DefaultValue( 0f )]
		public float AdditionalInjuryPerDamagingHit = 0f;

		[Label( "Max health lost from injury buffer fill" )]
		[Range( 0, 100 )]
		[DefaultValue( 5 )]
		public int MaxHealthLostFromInjury = 5;


		[Label( "Lowest allowed max health" )]
		[DrawTicks]
		[Range( 5, 100 )]
		[Increment( 5 )]
		[DefaultValue( 20 )]
		public int LowestAllowedMaxHealth = 20;


		[Tooltip( "In seconds." )]
		[Label( "Injury buffer auto-heal rate" )]
		[Range( 60, 60 * 10 )]  // Up to 10 minutes
		[DefaultValue( 75 )]
		public int InjuryBufferHealRate = 75;  // 1 hp every 75 seconds

		[Tooltip( "In seconds." )]
		[Label( "Injury buffer auto-heal rate with Band of Life" )]
		[Range( 60, 60 * 10 )]  // Up to 10 minutes
		[DefaultValue( 30 )]
		public int BandOfLifeInjuryHealRate = 30;  // 1 hp every 30 seconds

		[Tooltip( "In seconds." )]
		[Label( "Injury buffer auto-heal rate with Band of Afterlife" )]
		[Range( 1, 60 * 10 )]  // Up to 10 minutes
		[DefaultValue( 45 )]
		public int BandOfAfterlifeInjuryHealRate = 45;	// 1 hp every 45 seconds


		[Label( "Reduce injury with high max health" )]
		[DefaultValue( true )]
		public bool HighMaxHealthReducesInjury = true;


		[Tooltip( "Multiplies hp lost as the duration. Duration is in ticks (1/60 second units)." )]
		[Label( "Fall limp duration multiplier" )]
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

		//[SeparatePage]
		
		[Label( "Drop Broken Hearts on injury" )]
		[DefaultValue( true )]
		public bool BrokenHeartsDrop = true;

		[Tooltip( "In seconds." )]
		[Label( "Duration of broken hearts (1/60s units)" )]
		[Range( 1, 60 * 10 )]	// 10 minutes
		[DefaultValue( 24 )]
		public int BleedingHeartDuration = 24;


		[Label( "Vitae required to craft Life Crystal" )]
		[Range( 0, 100 )]
		[DefaultValue( 4 )]
		public int VitaePerLifeCrystal = 4;

		[Label( "Vitae required to craft Cracked Life Crystal" )]
		[Range( 0, 100 )]
		[DefaultValue( 2 )]
		public int VitaePerCrackedLifeCrystal = 2;

		[Label( "Vitae required to craft Cracked Life Crystal" )]
		[Range( 0, 100 )]
		[DefaultValue( 3 )]
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

		//[SeparatePage]

		[Tooltip( "Time until 5 max hp is lost. Measured in seconds." )]
		[Label( "Cracked Life Crystal's temp. max hp time" )]
		[Range( 1, 15 * 60 )]	// 15 minutes
		[DefaultValue( 5 * 30 )]
		public int TemporaryMaxHpChunkDrainRate = 5 * 30;   // Every 30 seconds


		[Tooltip( "Measured as % of hp." )]
		[Label( "Life remaining until Bleeding debuff appears" )]
		[Range( 0f, 1f )]
		[Increment( 0.01f )]
		[DefaultValue( 0.35f )]
		public float MaxHpPercentRemainingUntilBleeding = 0.35f;

		[Tooltip( "Measured as % of hp." )]
		[Label( "Life remaining before powerful blows stagger" )]
		[Range( 0f, 1f )]
		[Increment( 0.01f )]
		[DefaultValue( 0.25f )]
		public float MaxHpPercentLossForPowerfulBlowStagger = 0.25f;


		[Tooltip( "Measured as % of max hp." )]
		[Label( "Damage before injury happens while at full health" )]
		[Range( 0f, 1f )]
		[Increment( 0.01f )]
		[DefaultValue( 0.20f )]
		public float MaxHpPercentAsDamageAtFullHealthUntilHarm = 0.20f; // Adventurer's grace

		//[SeparatePage]

		[Tooltip( "In seconds." )]
		[Label( "Broken Heart added duration from Heartstrings" )]
		[Range( 0, 60 * 10 )]	// 10 minutes
		[DefaultValue( 45 )]
		public int HeartstringsAddedDuration = 45;

		[Label( "Fortitude potion injury buffer size multiplier" )]
		[Range( 1f, 10f )]
		[Increment( 0.01f )]
		[DefaultValue( 1.4f )]
		public float FortitudePotionHarmBufferMultiplier = 1.4f;

		[Label( "Life vest injury buffer size multiplier" )]
		[Range( 1f, 10f )]
		[Increment( 0.01f )]
		[DefaultValue( 1.7f )]
		public float LifeVestHarmBufferMultiplier = 1.7f;


		[Tooltip( "Larger is safer." )]
		[Label( "Odds of \"accident\" while crafting Vitae" )]
		[Range( 1, 100 )]
		[DefaultValue( 6 )]
		public int VitaeCraftingAccidentOdds = 6;
	}
}
