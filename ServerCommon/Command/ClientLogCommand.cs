using GameCommon;
using SuperSocket.SocketBase.Command;

namespace ServerCommon.Command
{
	public class ClientLogCommand : CommandBase<GameSession, GameMessage>
	{
		private static GameLogger m_logger = new GameLogger(log4net.LogManager.GetLogger("ClientLog"));

		// todo_tianyu: clientlog file
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			GameServer Server = session.AppServer as GameServer;
			var packet = requestInfo.Packet as ClientLogPacket;

			m_logger.LogMessage(packet.m_logType, packet.m_message);
		}
	}
}
