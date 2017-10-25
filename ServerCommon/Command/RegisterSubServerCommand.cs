using GameCommon;
using SuperSocket.SocketBase.Command;

namespace ServerCommon.Command
{
	public class RegisterSubServerCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			GameServer Server = session.AppServer as GameServer;
			var packet = requestInfo.Packet as RegisterSubServerPacket;

			var resultPacket = new RegisterSubServerResultPacket()
			{
				m_serverType = Server.ServerType,
				m_serverId = Server.ServerId,
				m_clientPort = Server.ClientPort
			};

			if (Server.Connections.SubServers.ContainsKey(packet.m_serverId) == true)
			{
				resultPacket.m_result = RegisterSubServerResultPacket.ResultEnum.Registered;
			}
			else
			{
				resultPacket.m_result = RegisterSubServerResultPacket.ResultEnum.Ok;

				session.ServerType = packet.m_serverType;
				session.PeerId = packet.m_serverId;
				session.ClientPort = packet.m_clientPort;
				Server.Connections.OnSubServerRegister(session);

				session.SendPacket(resultPacket);
			}
		}
	}
}
