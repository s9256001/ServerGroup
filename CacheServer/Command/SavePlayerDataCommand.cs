using CacheServer.Models;
using CacheServer.Server;
using GameCommon;
using ServerCommon;
using SuperSocket.SocketBase.Command;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CacheServer.Command
{
	public class SavePlayerDataCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as SavePlayerDataPacket;

			// get player
			PlayerModel playerModel = null;
			using (var db = new GameDbContext())
			{
				playerModel = db.Players.FirstOrDefault(p => p.PlayerId == packet.m_playerData.PlayerId);
			}
			// player not existed
			if (playerModel == null)
			{
				Server.GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error,
					"Player not existed! playerId = {0}", packet.m_playerData.PlayerId);
				return;
			}

			// save PlayerModel
			using (var db = new GameDbContext())
			{
				playerModel.LoginTime = packet.m_playerData.LoginTime;
				playerModel.LogoutTime = packet.m_playerData.LogoutTime;

				db.Entry<PlayerModel>(playerModel).State = EntityState.Modified;
				try
				{
					db.SaveChanges();
				}
				catch (Exception e)
				{
					Server.GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error,
						"Save PlayerModel failed! exception = {0}", e.ToString());
					return;
				}
			}

			// save RoleModel
			List<RoleModel> roleModels = null;
			List<RoleData> roleDatas = new List<RoleData>(packet.m_playerData.RoleDatas);
			using (var db = new GameDbContext())
			{
				roleModels = db.Roles.Where(c => c.PlayerId == playerModel.PlayerId).ToList();

				foreach (var roleModel in roleModels)
				{
					RoleData roleData = roleDatas.FirstOrDefault(c => c.RoleId == roleModel.RoleId);
					// delete
					if (roleData == null)
					{
						db.Roles.Remove(roleModel);
					}
					// modify
					else
					{
						db.Entry<RoleModel>(roleModel).State = EntityState.Modified;
						roleDatas.Remove(roleData);
					}
				}
				// insert
				foreach (var roleData in roleDatas)
				{
					RoleModel roleModel = new RoleModel()
					{
						RoleId = roleData.RoleId,
						Name = roleData.Name,
						Image = roleData.Image.ToArray(),
						CreateTime = roleData.CreateTime,
						PlayerId = roleData.PlayerId
					};
					db.Roles.Add(roleModel);
				}
				try
				{
					db.SaveChanges();
				}
				catch (Exception e)
				{
					Server.GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error,
						"Save RoleModel failed! exception = {0}", e.ToString());
					return;
				}
			}
		}
	}
}
