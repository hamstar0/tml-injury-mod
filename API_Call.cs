using System;


namespace Injury {
	public static partial class InjuryAPI {
		internal static object Call( string call_type, params object[] args ) {
			switch( call_type ) {
			case "GetModClientSettings":
				return InjuryAPI.GetModClientSettings();
			case "GetModServerSettings":
				return InjuryAPI.GetModServerSettings();
				return null;
			}

			throw new Exception( "No such api call " + call_type );
		}
	}
}
