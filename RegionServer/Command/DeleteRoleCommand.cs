using GameCommon;
using RegionServer.Data;
using RegionServer.Server;
using ServerCommon;
using SuperSocket.SocketBase.Command;
using System.Linq;

namespace RegionServer.Command
{
	public class DeleteRoleCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as DeleteRolePacket;

			var resultPacket = new DeleteRoleResultPacket()
			{
				m_result = DeleteRoleResultPacket.ResultEnum.Ok
			};

			// no player data
			DerivedPlayerData playerData = null;
			if (Server.PlayerDatas.TryGetValue(packet.m_playerKey, out playerData) == false)
			{
				resultPacket.m_result = DeleteRoleResultPacket.ResultEnum.Failed;
				session.SendPacket(resultPacket);
				return;
			}

			// no character
			var roleData = playerData.RoleDatas.FirstOrDefault(cd => cd.RoleId == packet.m_roleId);
			if (roleData == null)
			{
				resultPacket.m_result = DeleteRoleResultPacket.ResultEnum.NoCharacter;
				session.SendPacket(resultPacket);
				return;
			}

			playerData.RoleDatas.Remove(roleData);
			resultPacket.m_roleId = packet.m_roleId;
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
