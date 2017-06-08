using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Injury {
	public enum InjuryNetProtocolTypes : byte {
		ModSettingsRequest,
		ModSettings
	}


	public static class InjuryNetProtocol {
		public static void RoutePacket( InjuryMod mymod, BinaryReader reader ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case InjuryNetProtocolTypes.ModSettingsRequest:
				InjuryNetProtocol.ReceiveSettingsRequestOnServer( mymod, reader );
				break;
			case InjuryNetProtocolTypes.ModSettings:
				InjuryNetProtocol.ReceiveSettingsOnClient( mymod, reader );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////////////////////
		// Senders (client)
		////////////////////////////////

		public static void SendSettingsRequestFromClient( InjuryMod mymod, Player player ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.ModSettingsRequest );
			packet.Write( (int)player.whoAmI );
			packet.Send();
		}

		////////////////////////////////
		// Senders (server)
		////////////////////////////////

		private static void SendSettingFromServer( InjuryMod mymod, Player player ) {
			if( Main.netMode != 2 ) { return; }	// Server only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.Config.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}



		////////////////////////////////
		// Recipients (client)
		////////////////////////////////

		private static void ReceiveSettingsOnClient( InjuryMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Client only
			
			mymod.Config.DeserializeMe( reader.ReadString() );
		}

		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveSettingsRequestOnServer( InjuryMod mymod, BinaryReader reader ) {
			if( Main.netMode != 2 ) { return; } // Server only

			int who = reader.ReadInt32();
			if( who < 0 || who >= Main.player.Length || Main.player[who] == null ) {
				ErrorLogger.Log( "InjuryNetProtocol.ReceiveSettingsRequestOnServer - Invalid player whoAmI. " + who );
				return;
			}
			
			InjuryNetProtocol.SendSettingFromServer( mymod, Main.player[who] );
		}

	}
}
