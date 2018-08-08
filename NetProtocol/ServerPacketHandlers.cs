using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Injury.NetProtocol {
	static class ServerPacketHandlers {
		public static void RoutePacket( InjuryMod mymod, BinaryReader reader, int player_who ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case InjuryNetProtocolTypes.ModSettingsRequest:
				ServerPacketHandlers.ReceiveSettingsRequest( mymod, reader, player_who );
				break;
			case InjuryNetProtocolTypes.NpcSpawnRequest:
				ServerPacketHandlers.ReceiveNpcSpawnRequest( mymod, reader, player_who );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}

		
		////////////////////////////////
		// Senders
		////////////////////////////////

		private static void SendSettings( InjuryMod mymod, Player player ) {
			ModPacket packet = mymod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.ModSettings );
			packet.Write( (string)mymod.ConfigJson.SerializeMe() );

			packet.Send( (int)player.whoAmI );
		}
		

		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveSettingsRequest( InjuryMod mymod, BinaryReader reader, int player_who ) {
			ServerPacketHandlers.SendSettings( mymod, Main.player[player_who] );
		}

		private static void ReceiveNpcSpawnRequest( InjuryMod mymod, BinaryReader reader, int player_who ) {
			int npc_id = reader.ReadInt32();

			NPC.SpawnOnPlayer( player_who, npc_id );
		}
	}
}
