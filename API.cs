using System;


namespace Injury {
	public static partial class InjuryAPI {
		[Obsolete( "use GetModServerSettings" )]
		public static InjuryServerConfigData GetModSettings() {
			return InjuryAPI.GetModServerSettings();
		}


		public static InjuryClientConfigData GetModClientSettings() {
			return InjuryMod.Instance.ClientConfig;
		}
		public static InjuryServerConfigData GetModServerSettings() {
			return InjuryMod.Instance.ServerConfig;
		}
	}
}
