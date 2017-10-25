using GameCommon;

namespace ServerCommon
{
	public abstract class SubServerBase : GameServer
	{
		public override void Register(GameSession session)
		{
			if (session == null)
			{
				return;
			}

			var packet = new RegisterSubServerPacket()
			{
				m_serverType = ServerType,
				m_serverId = ServerId,
				m_clientPort = ClientPort
			};
			session.SendPacket(packet);
		}
	}
}
