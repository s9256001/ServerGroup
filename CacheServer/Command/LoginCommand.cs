using CacheServer.Models;
using CacheServer.Server;
using GameCommon;
using ServerCommon;
using SuperSocket.SocketBase.Command;
using System;
using System.Data.Entity;
using System.Linq;

namespace CacheServer.Command
{
	public class LoginCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as LoginPacket;

			var resultPacket = new LoginResultPacket()
			{
				m_clientSessionId = packet.m_clientSessionId
			};

			// todo_tianyu: encrypt password

			// get account
			AccountModel accountModel = null;
			using (var db = new GameDbContext())
			{
				accountModel = db.Accounts.FirstOrDefault(a => a.Account == packet.m_account && a.Password == packet.m_password);
			}
			// account not existed
			if (accountModel == null)
			{
				resultPacket.m_result = LoginResultPacket.ResultEnum.Failed;
				session.SendPacket(resultPacket);
				return;
			}
			// get player
			PlayerModel playerModel = null;
			using (var db = new GameDbContext())
			{
				playerModel = db.Players.FirstOrDefault(p => p.PlayerId == accountModel.PlayerId);
			}
			// player not existed
			if (playerModel == null)
			{
				Server.GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error,
						"Player not existed! playerId = {0}", accountModel.PlayerId);

				resultPacket.m_result = LoginResultPacket.ResultEnum.Failed;
				session.SendPacket(resultPacket);
				return;
			}

			// select a region server
			var regionSession = Server.Connections.SubServers.Values.FirstOrDefault(s => s.ServerType == ServerTypeEnum.Region);
			if (regionSession == null)
			{
				resultPacket.m_result = LoginResultPacket.ResultEnum.NoRegion;
				session.SendPacket(resultPacket);
				return;
			}
			// todo_tianyu: setting
			resultPacket.m_regionAddress = Server.PublicServerAddress;
			resultPacket.m_regionPort = regionSession.ClientPort;

			// set login time
			using (var db = new GameDbContext())
			{
				playerModel.LoginTime = DateTime.Now;

				db.Entry<PlayerModel>(playerModel).State = EntityState.Modified;
				try
				{
					db.SaveChanges();
				}
				catch (Exception e)
				{
					Server.GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error,
						"Update login time failed! exception = {0}", e.ToString());

					resultPacket.m_result = LoginResultPacket.ResultEnum.Failed;
					session.SendPacket(resultPacket);
					return;
				}
			}

			// create player key
			Guid playerKey = Guid.NewGuid();
			PlayerData playerData = new PlayerData()
			{
				PlayerId = playerModel.PlayerId,
				PlayerKey = playerKey
			};
			Server.PlayerDatas[playerKey] = playerData;
			resultPacket.m_playerKey = playerData.PlayerKey;

			resultPacket.m_result = LoginResultPacket.ResultEnum.Ok;
			session.SendPacket(resultPacket);
		}
	}
}
