using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Injury.NetProtocol {
	static class ServerPacketHandlers {
		public static void RoutePacket( InjuryMod mymod, BinaryReader reader, int player_who ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case InjuryNetProtocolTypes.ModSettingsRequest:
				ServerPacketHandlers.ReceiveSettingsRequestOnServer( mymod, reader, player_who );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}

		
		////////////////////////////////
		// Senders (server)
		////////////////////////////////

		private static void SendSettingFromServer( InjuryMod mymod, Player player ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.Config.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}
		

		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveSettingsRequestOnServer( InjuryMod mymod, BinaryReader reader, int player_who ) {
			if( Main.netMode != 2 ) { return; } // Server only

			ServerPacketHandlers.SendSettingFromServer( mymod, Main.player[player_who] );
		}
	}
}
