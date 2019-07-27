using HamstarHelpers.Helpers.Debug;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Injury.NetProtocol {
	static class ServerPacketHandlers {
		public static void RoutePacket( InjuryMod mymod, BinaryReader reader, int playerWho ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case InjuryNetProtocolTypes.NpcSpawnRequest:
				ServerPacketHandlers.ReceiveNpcSpawnRequest( mymod, reader, playerWho );
				break;
			default:
				LogHelpers.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}

		
		////////////////////////////////
		// Senders
		////////////////////////////////
		

		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveNpcSpawnRequest( InjuryMod mymod, BinaryReader reader, int playerWho ) {
			int npcId = reader.ReadInt32();

			NPC.SpawnOnPlayer( playerWho, npcId );
		}
	}
}
