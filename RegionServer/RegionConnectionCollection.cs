using GameCommon;
using RegionServer.Server;
using ServerCommon;
using System.Linq;

namespace RegionServer
{
	public class RegionConnectionCollection : ConnectionCollection
	{
		public RegionConnectionCollection(GameServer server) : base(server)
        {

        }

		/// <summary>
		/// Client斷線
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public override void ClientDisconnect(GameSession session)
		{
			DerivedServer server = Server as DerivedServer;

			var playerData = server.PlayerDatas.Values.FirstOrDefault(pd => pd.Session == session);
			if (playerData == null)
			{
				return;
			}

			var packet = new LogoutPacket()
			{
				m_playerKey = playerData.PlayerKey
			};
			Server.MasterSession.SendPacket(packet);

			server.PlayerDatas.Remove(playerData.PlayerKey);
		}
	}
}
