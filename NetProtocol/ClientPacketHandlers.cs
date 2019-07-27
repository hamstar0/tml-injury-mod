using HamstarHelpers.Helpers.Debug;
using System.IO;
using Terraria.ModLoader;


namespace Injury.NetProtocol {
	static class ClientPacketHandlers {
		public static void RoutePacket( InjuryMod mymod, BinaryReader reader ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}

		
		////////////////////////////////
		// Senders
		////////////////////////////////

		public static void SendSpawnRequest( InjuryMod mymod, int npcType ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.NpcSpawnRequest );
			packet.Write( (int)npcType );
			packet.Send();
		}



		////////////////////////////////
		// Recipients
		////////////////////////////////
	}
}
