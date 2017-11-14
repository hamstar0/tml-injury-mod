namespace Injury {
	public static class InjuryAPI {
		public static InjuryConfigData GetModSettings() {
			return InjuryMod.Instance.Config.Data;
		}
	}
}
