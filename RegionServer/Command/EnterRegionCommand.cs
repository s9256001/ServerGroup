using GameCommon;
using RegionServer.Server;
using ServerCommon;
using SuperSocket.SocketBase.Command;

namespace RegionServer.Command
{
	public class EnterRegionCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as EnterRegionPacket;

			packet.m_clientSessionId = session.SessionID;
			Server.MasterSession.SendPacket(packet);
		}
	}
}
