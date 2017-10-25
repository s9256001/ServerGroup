using GameCommon;
using RegionServer.Data;
using RegionServer.Server;
using ServerCommon;
using SuperSocket.SocketBase.Command;
using System;

namespace RegionServer.Command
{
	public class CreateRoleCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as CreateRolePacket;

			var resultPacket = new CreateRoleResultPacket()
			{
				m_result = CreateRoleResultPacket.ResultEnum.Ok
			};

			// no player data
			DerivedPlayerData playerData = null;
			if (Server.PlayerDatas.TryGetValue(packet.m_playerKey, out playerData) == false)
			{
				resultPacket.m_result = CreateRoleResultPacket.ResultEnum.Failed;
				session.SendPacket(resultPacket);
				return;
			}

			// no role space
			if (playerData.RoleDatas.Count >= PlayerData.MaxRoleNum)
			{
				resultPacket.m_result = CreateRoleResultPacket.ResultEnum.NoRoleSpace;
				session.SendPacket(resultPacket);
				return;
			}

			// invalid name
			// todo_tianyu: name invalid characters check
			if (string.IsNullOrEmpty(packet.m_name) == true)
			{
				resultPacket.m_result = CreateRoleResultPacket.ResultEnum.InvalidName;
				session.SendPacket(resultPacket);
				return;
			}
			if (packet.m_name.Length > PlayerData.MaxRoleNameLength)
			{
				resultPacket.m_result = CreateRoleResultPacket.ResultEnum.InvalidName;
				session.SendPacket(resultPacket);
				return;
			}

			RoleData roleData = new RoleData()
			{
				RoleId = Guid.NewGuid(),
				Name = packet.m_name,
				Image = packet.m_image,
				CreateTime = DateTime.Now,
				PlayerId = playerData.PlayerId
			};
			playerData.RoleDatas.Add(roleData);
			resultPacket.m_roleData = roleData;
			session.SendPacket(resultPacket);

			// todo_tianyu: save
			var savePlayerDataPacket = new SavePlayerDataPacket()
			{
				m_playerData = playerData
			};
			Server.MasterSession.SendPacket(savePlayerDataPacket);
		}
	}
}
