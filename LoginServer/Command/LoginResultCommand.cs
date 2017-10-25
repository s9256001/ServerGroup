﻿using GameCommon;
using LoginServer.Server;
using ServerCommon;
using SuperSocket.SocketBase.Command;

namespace LoginServer.Command
{
	public class LoginResultCommand : CommandBase<GameSession, GameMessage>
	{
		public override void ExecuteCommand(GameSession session, GameMessage requestInfo)
		{
			DerivedServer Server = session.AppServer as DerivedServer;
			var packet = requestInfo.Packet as LoginResultPacket;

			var clientSession = Server.GetSessionByID(packet.m_clientSessionId);
			clientSession.SendPacket(packet);
		}
	}
}
