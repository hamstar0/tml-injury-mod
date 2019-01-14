using System;
using Terraria;


namespace Injury {
	public static class InjuryAPI {
		public static InjuryConfigData GetModSettings() {
			return InjuryMod.Instance.Config;
		}

		public static void SaveModSettingsChanges() {
			InjuryMod.Instance.ConfigJson.SaveFile();
		}
	}
}
