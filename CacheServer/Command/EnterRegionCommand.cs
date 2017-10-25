using CacheServer.Models;
using CacheServer.Server;
using GameCommon;
using ServerCommon;
using SuperSocket.SocketBase.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CacheServer.Command
{
	public class EnterRegionCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as EnterRegionPacket;

			var resultPacket = new EnterRegionResultPacket()
			{
				m_clientSessionId = packet.m_clientSessionId
			};

			// no player key
			if (packet.m_playerKey == Guid.Empty)
			{
				resultPacket.m_result = EnterRegionResultPacket.ResultEnum.Failed;
				session.SendPacket(packet);
			}

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
				resultPacket.m_result = EnterRegionResultPacket.ResultEnum.Failed;
				session.SendPacket(resultPacket);
				return;
			}

			// get roles
			List<RoleModel> roleModels = null;
			using (var db = new GameDbContext())
			{
				roleModels = db.Roles.Where(c => c.PlayerId == playerModel.PlayerId).ToList();
			}

			resultPacket.m_playerData = new PlayerData()
			{
				PlayerKey = packet.m_playerKey,
				LoginTime = playerModel.LoginTime,
				LogoutTime = playerModel.LogoutTime,
				PlayerId = playerModel.PlayerId
			};
			resultPacket.m_playerData.RoleDatas = new List<RoleData>();
			foreach (var roleModel in roleModels)
			{
				RoleData roleData = new RoleData()
				{
					RoleId = roleModel.RoleId,
					Name = roleModel.Name,
					Image = new List<byte>(roleModel.Image),
					CreateTime = roleModel.CreateTime,
					PlayerId = roleModel.PlayerId
				};
				resultPacket.m_playerData.RoleDatas.Add(roleData);
			}

			resultPacket.m_result = EnterRegionResultPacket.ResultEnum.Ok;
			session.SendPacket(resultPacket);
		}
	}
}
