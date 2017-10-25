using CacheServer.Models;
using CacheServer.Server;
using GameCommon;
using ServerCommon;
using SuperSocket.SocketBase.Command;
using System;
using System.Data.SqlTypes;
using System.Linq;

namespace CacheServer.Command
{
	public class RegisterAccountCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as RegisterAccountPacket;

			var resultPacket = new RegisterAccountResultPacket()
			{
				m_clientSessionId = packet.m_clientSessionId
			};

			// get account
			AccountModel accountModel = null;
			using (var db = new GameDbContext())
			{
				accountModel = db.Accounts.FirstOrDefault(a => a.Account == packet.m_account);
			}
			// account not existed
			if (accountModel != null)
			{
				resultPacket.m_result = RegisterAccountResultPacket.ResultEnum.Registered;
				session.SendPacket(resultPacket);
				return;
			}

			// create account
			using (var db = new GameDbContext())
			{
				// create account
				accountModel = new AccountModel()
				{
					Account = packet.m_account,
					Password = packet.m_password,
					CreateTime = DateTime.Now
				};
				db.Accounts.Add(accountModel);
				// create player
				var playerModel = new PlayerModel()
				{
					PlayerId = accountModel.PlayerId,
					LoginTime = SqlDateTime.MinValue.Value,
					LogoutTime = SqlDateTime.MinValue.Value
				};
				db.Players.Add(playerModel);
				try
				{
					db.SaveChanges();
				}
				catch (Exception e)
				{
					Server.GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error,
						"Create account failed! exception = {0}", e.ToString());

					resultPacket.m_result = RegisterAccountResultPacket.ResultEnum.Failed;
					session.SendPacket(resultPacket);
					return;
				}
			}

			resultPacket.m_result = RegisterAccountResultPacket.ResultEnum.Ok;
			session.SendPacket(resultPacket);
		}
	}
}
