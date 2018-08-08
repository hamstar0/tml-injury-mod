using Terraria.ModLoader;
using Terraria;
using System.IO;
using System;
using HamstarHelpers.Components.Config;


namespace Injury {
	partial class InjuryMod : Mod {
		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-injury-mod"; } }

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + InjuryConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( InjuryMod.Instance != null ) {
				if( !InjuryMod.Instance.ConfigJson.LoadFile() ) {
					InjuryMod.Instance.ConfigJson.SaveFile();
				}
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var new_config = new InjuryConfigData();
			//new_config.SetDefaults();

			InjuryMod.Instance.ConfigJson.SetData( new_config );
			InjuryMod.Instance.ConfigJson.SaveFile();
		}


		////////////////

		public bool IsDebugInfoMode() {
			return this.Config.DebugModeInfo;
		}
	}
}
