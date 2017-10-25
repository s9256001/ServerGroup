using GameCommon;
using LoginServer.Server;
using ServerCommon;
using SuperSocket.SocketBase.Command;

namespace LoginServer.Command
{
	public class LoginCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as LoginPacket;

			packet.m_clientSessionId = session.SessionID;
			Server.MasterSession.SendPacket(packet);
		}
	}
}
