using Terraria.ModLoader;
using System;


namespace Injury {
	public partial class InjuryMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-injury-mod";


		////////////////

		public bool IsDebugInfoMode() {
			return this.Config.DebugModeInfo;
		}
	}
}
