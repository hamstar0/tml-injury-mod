using System;
using Terraria;


namespace Injury {
	public static class InjuryAPI {
		internal static object Call( string call_type, params object[] args ) {
			switch( call_type ) {
			case "GetModSettings":
				return InjuryAPI.GetModSettings();
			case "SaveModSettingsChanges":
				InjuryAPI.SaveModSettingsChanges();
				return null;
			}

			throw new Exception( "No such api call " + call_type );
		}



		////////////////

		public static InjuryConfigData GetModSettings() {
			return InjuryMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			InjuryMod.Instance.JsonConfig.SaveFile();
		}
	}
}
