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
	public class LogoutCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as LogoutPacket;

			// not login
			PlayerData playerData = null;
			if (Server.PlayerDatas.TryGetValue(packet.m_playerKey, out playerData) == false)
			{
				return;
			}

			// get player
			PlayerModel playerModel = null;
			using (var db = new GameDbContext())
			{
				playerModel = db.Players.FirstOrDefault(p => p.PlayerId == playerData.PlayerId);
			}
			// player not existed
			if (playerModel == null)
			{
				return;
			}

			// set logout time
			using (var db = new GameDbContext())
			{
				playerModel.LogoutTime = DateTime.Now;

				db.Entry<PlayerModel>(playerModel).State = EntityState.Modified;
				try
				{
					db.SaveChanges();
				}
				catch (Exception e)
				{
					Server.GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error,
						"Update logout time failed! exception = {0}", e.ToString());
					return;
				}
			}
		}
	}
}
