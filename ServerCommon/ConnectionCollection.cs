using System;
using System.Collections.Generic;

namespace ServerCommon
{
	/// <summary>
	/// 連線管理
	/// </summary>
	public class ConnectionCollection
	{
		/// <summary>
		/// Server
		/// </summary>
		public GameServer Server { get; set; }
		/// <summary>
		/// ServerId對應的子Server列表
		/// </summary>
		public Dictionary<Guid, GameSession> SubServers { get; set; }
		/// <summary>
		/// ClientId對應的Client列表
		/// </summary>
		public Dictionary<Guid, GameSession> Clients { get; set; }
		/// <summary>
		/// 遊戲Logger
		/// </summary>
		private CommonLib.Utility.Logger GameLogger { get; set; }

		public ConnectionCollection(GameServer server)
        {
			Server = server;
			SubServers = new Dictionary<Guid, GameSession>();
			Clients = new Dictionary<Guid, GameSession>();
			GameLogger = new GameLogger(log4net.LogManager.GetLogger(GetType().Name));
        }

		/// <summary>
		/// 子Server註冊
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public void OnSubServerRegister(GameSession session)
		{
			lock (SubServers)
			{
				SubServers.Add(session.PeerId, session);
				SubServerRegister(session);
			}
		}
		/// <summary>
		/// 子Server註冊
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public virtual void SubServerRegister(GameSession session)
		{

		}
		/// <summary>
		/// 子Server斷線
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public void OnSubServerDisconnect(GameSession session)
		{
			lock (SubServers)
			{
				SubServers.Remove(session.PeerId);
				SubServerDisconnect(session);
			}
		}
		/// <summary>
		/// 子Server斷線
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public virtual void SubServerDisconnect(GameSession session)
		{

		}
		/// <summary>
		/// Client註冊
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public void OnClientRegister(GameSession session)
		{
			lock (Clients)
			{
				Clients.Add(session.PeerId, session);
				ClientRegister(session);
			}
		}
		/// <summary>
		/// Client註冊
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public virtual void ClientRegister(GameSession session)
		{

		}
		/// <summary>
		/// Client斷線
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public void OnClientDisconnect(GameSession session)
		{
			lock (Clients)
			{
				Clients.Remove(session.PeerId);
				ClientDisconnect(session);
			}
		}
		/// <summary>
		/// Client斷線
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public virtual void ClientDisconnect(GameSession session)
		{

		}
		/// <summary>
		/// 切斷所有連線
		/// </summary>
		public void DisconnectAll()
		{
			foreach (var session in SubServers.Values)
			{
				session.Close();
			}
			foreach (var session in Clients.Values)
			{
				session.Close();
			}
			SubServers.Clear();
			Clients.Clear();
		}
	}
}
