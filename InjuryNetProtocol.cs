using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace Injury {
	public enum InjuryNetProtocolTypes : byte {
		SendSettingsRequest,
		SendSettings
	}


	public class InjuryNetProtocol {
		public static void RoutePacket( Mod mod, BinaryReader reader ) {
			InjuryNetProtocolTypes protocol = (InjuryNetProtocolTypes)reader.ReadByte();

			switch( protocol ) {
			case InjuryNetProtocolTypes.SendSettingsRequest:
				InjuryNetProtocol.ReceiveSettingsRequestOnServer( mod, reader );
				break;
			case InjuryNetProtocolTypes.SendSettings:
				InjuryNetProtocol.ReceiveSettingsOnClient( mod, reader );
				break;
			default:
				ErrorLogger.Log( "Invalid packet protocol: " + protocol );
				break;
			}
		}



		////////////////////////////////
		// Senders (client)
		////////////////////////////////

		public static void SendSettingsRequestFromClient( Mod mod, Player player ) {
			if( Main.netMode != 1 ) { return; } // Clients only

			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.SendSettingsRequest );
			packet.Write( (int)player.whoAmI );
			packet.Send();
		}

		////////////////////////////////
		// Senders (server)
		////////////////////////////////

		private static void SendSettingFromServer( Mod mod, Player player ) {
			if( Main.netMode != 2 ) { return; }	// Server only

			ModPacket packet = mod.GetPacket();

			packet.Write( (byte)InjuryNetProtocolTypes.SendSettings );
			packet.Write( (float)InjuryMod.Config.Data.PercentOfDamageToUseAsHarm );
			packet.Write( (int)InjuryMod.Config.Data.FallLimpDurationMultiplier );
			packet.Write( (float)InjuryMod.Config.Data.FallLimpSpeedMultiplier );
			packet.Write( (float)InjuryMod.Config.Data.FallLimpJumpMultiplier );
			packet.Write( (int)InjuryMod.Config.Data.LowestAllowedMaxHealth );
			packet.Write( (float)InjuryMod.Config.Data.HarmHealPerSecond );
			packet.Write( (float)InjuryMod.Config.Data.BandOfLifeHarmHealPerSecond );
			packet.Write( (float)InjuryMod.Config.Data.AdditionalHarmPerDamagingHit );
			packet.Write( (float)InjuryMod.Config.Data.HarmBeforeReceivingInjury );
			packet.Write( (bool)InjuryMod.Config.Data.HighMaxHealthReducesReceivedHarm );
			packet.Write( (int)InjuryMod.Config.Data.MaxHealthLostFromInjury );
			packet.Write( (bool)InjuryMod.Config.Data.BrokenHeartsDrop );
			
			packet.Send( (int)player.whoAmI );
		}



		////////////////////////////////
		// Recipients (client)
		////////////////////////////////

		private static void ReceiveSettingsOnClient( Mod mod, BinaryReader reader ) {
			if( Main.netMode != 1 ) { return; } // Client only

			InjuryMod.Config.Data.PercentOfDamageToUseAsHarm = (float)reader.ReadSingle();
			InjuryMod.Config.Data.FallLimpDurationMultiplier = (int)reader.ReadInt32();
			InjuryMod.Config.Data.FallLimpSpeedMultiplier = (float)reader.ReadSingle();
			InjuryMod.Config.Data.FallLimpJumpMultiplier = (float)reader.ReadSingle();
			InjuryMod.Config.Data.LowestAllowedMaxHealth = (int)reader.ReadInt32();
			InjuryMod.Config.Data.HarmHealPerSecond = (float)reader.ReadSingle();
			InjuryMod.Config.Data.BandOfLifeHarmHealPerSecond = (float)reader.ReadSingle();
			InjuryMod.Config.Data.AdditionalHarmPerDamagingHit = (float)reader.ReadSingle();
			InjuryMod.Config.Data.HarmBeforeReceivingInjury = (float)reader.ReadSingle();
			InjuryMod.Config.Data.HighMaxHealthReducesReceivedHarm = (bool)reader.ReadBoolean();
			InjuryMod.Config.Data.MaxHealthLostFromInjury = (int)reader.ReadInt32();
			InjuryMod.Config.Data.BrokenHeartsDrop = (bool)reader.ReadBoolean();
		}

		////////////////////////////////
		// Recipients (server)
		////////////////////////////////

		private static void ReceiveSettingsRequestOnServer( Mod mod, BinaryReader reader ) {
			if( Main.netMode != 2 ) { return; } // Server only

			int who = reader.ReadInt32();
			if( who < 0 || who >= Main.player.Length || Main.player[who] == null ) {
				ErrorLogger.Log( "InjuryNetProtocol.ReceiveSettingsRequestOnServer - Invalid player whoAmI. " + who );
				return;
			}
			
			InjuryNetProtocol.SendSettingFromServer( mod, Main.player[who] );
		}

	}
}
