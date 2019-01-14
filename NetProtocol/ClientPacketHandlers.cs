using System.IO;
using Terraria.ModLoader;


namespace Injury.NetProtocol {
	static class ClientPacketHandlers {
		public static void RoutePacket( InjuryMod mymod, BinaryReader reader ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case InjuryNetProtocolTypes.ModSettings:
				ClientPacketHandlers.ReceiveSettings( mymod, reader );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}

		
		////////////////////////////////
		// Senders
		////////////////////////////////

		public static void SendSettingsRequest( InjuryMod mymod ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.ModSettingsRequest );
			packet.Send();
		}

		public static void SendSpawnRequest( InjuryMod mymod, int npcType ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.NpcSpawnRequest );
			packet.Write( (int)npcType );
			packet.Send();
		}



		////////////////////////////////
		// Recipients
		////////////////////////////////

		private static void ReceiveSettings( InjuryMod mymod, BinaryReader reader ) {
			bool success;
			mymod.ConfigJson.DeserializeMe( reader.ReadString(), out success );
		}
	}
}
