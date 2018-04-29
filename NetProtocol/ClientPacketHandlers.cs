using System.IO;
using Terraria.ModLoader;


namespace Injury.NetProtocol {
	static class ClientPacketHandlers {
		public static void RoutePacket( InjuryMod mymod, BinaryReader reader ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}

		
		////////////////////////////////
		// Senders
		////////////////////////////////

		public static void SendSpawnRequestFromClient( InjuryMod mymod, int npc_type ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.NpcSpawnRequest );
			packet.Write( (int)npc_type );
			packet.Send();
		}



		////////////////////////////////
		// Recipients
		////////////////////////////////
	}
}
