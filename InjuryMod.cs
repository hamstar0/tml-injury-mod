using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Utils;
using Utils.JsonConfig;
using System.IO;
using System;


namespace Injury {
	public class ConfigurationData {
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

		public float InjuryBufferHealPerSecond = 1f / (60f * 75f);	// 1 hp every 75 seconds
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

		public float MaxHpPercentAsDamageAtFullHealthUntilHarm = 0.20f;	// Adventurer's grace
	}



	public class InjuryMod : Mod {
		public readonly static Version ConfigVersion = new Version(1, 9, 4);
		public JsonConfig<ConfigurationData> Config { get; private set; }

		public Texture2D HeartTex { get; private set; }



		public InjuryMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			string filename = "Injury Config.json";
			this.Config = new JsonConfig<ConfigurationData>( filename, "Mod Configs", new ConfigurationData() );
		}


		private void LoadConfig() {
			// Destroy ancient config
			var very_old_config = new JsonConfig<ConfigurationData>( "Injury 1.6.0.json", "", new ConfigurationData() );
			if( very_old_config.LoadFile() ) { very_old_config.DestroyFile(); }

			// Update old config to new location
			var old_config = new JsonConfig<ConfigurationData>( this.Config.FileName, "", new ConfigurationData() );
			if( old_config.LoadFile() ) {
				old_config.DestroyFile();
				old_config.SetFilePath( this.Config.FileName, "Mod Configs" );
				this.Config = old_config;
			} else if( !this.Config.LoadFile() ) {
				this.Config.SaveFile();
			} else {
				Version vers_since = this.Config.Data.VersionSinceUpdate != "" ?
					new Version( this.Config.Data.VersionSinceUpdate ) :
					new Version();

				if( vers_since < InjuryMod.ConfigVersion ) {
					var new_config = new ConfigurationData();
					ErrorLogger.Log( "Stamina config updated to " + InjuryMod.ConfigVersion.ToString() );

					if( vers_since < new Version( 1, 8, 1 ) ) {
						this.Config.Data.BandOfLifeInjuryHealPerSecond = new_config.BandOfLifeInjuryHealPerSecond;
					}
					if( vers_since < new Version( 1, 9, 2 ) ) {
						this.Config.Data.DurationOfBleedingHeart = new_config.DurationOfBleedingHeart;
					}
					
					this.Config.Data.VersionSinceUpdate = InjuryMod.ConfigVersion.ToString();
					this.Config.SaveFile();
				}
			}
		}

		public override void Load() {
			if( Main.netMode != 2 ) {	// Not server
				this.HeartTex = ModLoader.GetTexture( "Terraria/Heart" );
			}

			this.LoadConfig();
		}



		////////////////

		public override void HandlePacket( BinaryReader reader, int whoAmI ) {
			InjuryNetProtocol.RoutePacket( this, reader );
		}

		public override void AddRecipeGroups() {
			RecipeGroup group = new RecipeGroup( () => Lang.misc[37] + " Evil Biome Boss Drop", new int[] { 86, 1329 } );
			RecipeGroup.RegisterGroup( "InjuryMod:EvilBiomeBossDrop", group );
		}
		

		////////////////

		public override void PostDrawInterface( SpriteBatch sb ) {
			if( this.IsAnimatingHeartDrop ) {
				this.AnimateHeartDrop( sb, this.HeartDropAnimation++, 32 );
			
				if( this.HeartDropAnimation > 16 ) {
					this.HeartDropAnimation = 0;
					this.IsAnimatingHeartDrop = false;
				}
			}

			DebugHelper.PrintToBatch( sb );
		}


		////////////////

		private void AnimateHeartDrop( SpriteBatch sb, int frame, int maxframes ) {
			int x = 0;
			int y = 0;

			PlayerHelper.GetTopHeartPosition( Main.player[Main.myPlayer], ref x, ref y );
			y += frame * 2;

			Rectangle rect = new Rectangle( x, y, this.HeartTex.Width, this.HeartTex.Height );
			float alpha = (0.5f - (frame / maxframes) / 2);

			sb.Draw( this.HeartTex, rect, Color.White * alpha );
		}


		private bool IsAnimatingHeartDrop = false;
		private int HeartDropAnimation = 0;

		public void AnimateHeartDrop() {
			this.IsAnimatingHeartDrop = true;
		}

	}
}
