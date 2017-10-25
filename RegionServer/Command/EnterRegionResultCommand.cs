using GameCommon;
using RegionServer.Data;
using RegionServer.Server;
using ServerCommon;
using SuperSocket.SocketBase.Command;
using System;

namespace RegionServer.Command
{
	public class EnterRegionResultCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as EnterRegionResultPacket;

			var clientSession = Server.GetSessionByID(packet.m_clientSessionId);

			if (packet.m_result == EnterRegionResultPacket.ResultEnum.Ok)
			{
				var playerData = new DerivedPlayerData()
				{
					PlayerId = packet.m_playerData.PlayerId,
					PlayerKey = packet.m_playerData.PlayerKey,
					LoginTime = packet.m_playerData.LoginTime,
					LogoutTime = packet.m_playerData.LogoutTime,
					RoleDatas = packet.m_playerData.RoleDatas,
					Session = clientSession
				};
				Server.PlayerDatas[playerData.PlayerKey] = playerData;

				session.PeerId = Guid.NewGuid();
				Server.Connections.OnClientRegister(session);
			}

			clientSession.SendPacket(packet);
		}
	}
}
