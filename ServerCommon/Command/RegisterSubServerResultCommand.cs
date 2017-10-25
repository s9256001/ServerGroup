using GameCommon;
using SuperSocket.SocketBase.Command;

namespace ServerCommon.Command
{
	public class RegisterSubServerResultCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			GameServer Server = session.AppServer as GameServer;
			var packet = requestInfo.Packet as RegisterSubServerResultPacket;

			session.ServerType = packet.m_serverType;
			session.PeerId = packet.m_serverId;
			Server.MasterSession = session;
			Server.OnRegistered();
		}
	}
}
