using HamstarHelpers.DebugHelpers;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Injury.NetProtocol {
	static class ClientPacketHandlers {
		public static void RoutePacket( InjuryMod mymod, BinaryReader reader ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case InjuryNetProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveSettingsOnClient( mymod, reader );
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
			packet.Send();
		}



		////////////////////////////////
		// Recipients (client)
		////////////////////////////////

		private static void ReceiveSettingsOnClient( InjuryMod mymod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Client only

			mymod.Config.DeserializeMe( reader.ReadString() );
		}
	}
}
