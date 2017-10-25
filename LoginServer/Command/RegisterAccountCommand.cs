using GameCommon;
using LoginServer.Server;
using ServerCommon;
using SuperSocket.SocketBase.Command;

namespace LoginServer.Command
{
	public class RegisterAccountCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as RegisterAccountPacket;

			var resultPacket = new RegisterAccountResultPacket()
			{
				m_result = RegisterAccountResultPacket.ResultEnum.Ok
			};
			// todo_tianyu: account & password invalid characters check
			if (string.IsNullOrEmpty(packet.m_account) == true)
			{
				resultPacket.m_result = RegisterAccountResultPacket.ResultEnum.InvalidAccount;
				session.SendPacket(resultPacket);
				return;
			}
			if (packet.m_account.Length > PlayerData.MaxAccountLength || packet.m_account.Length < PlayerData.MinAccountLength)
			{
				resultPacket.m_result = RegisterAccountResultPacket.ResultEnum.InvalidAccount;
				session.SendPacket(resultPacket);
				return;
			}
			if (string.IsNullOrEmpty(packet.m_password) == true)
			{
				resultPacket.m_result = RegisterAccountResultPacket.ResultEnum.InvalidPassword;
				session.SendPacket(resultPacket);
				return;
			}
			if (packet.m_password.Length > PlayerData.MaxPasswordLength || packet.m_password.Length < PlayerData.MinPasswordLength)
			{
				resultPacket.m_result = RegisterAccountResultPacket.ResultEnum.InvalidPassword;
				session.SendPacket(resultPacket);
				return;
			}

			packet.m_clientSessionId = session.SessionID;
			Server.MasterSession.SendPacket(packet);
		}
	}
}
