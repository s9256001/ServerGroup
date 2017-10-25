using GameCommon;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace ServerCommon
{
	public abstract class GameServer : AppServer<GameSession, GameMessage>
	{
		/// <summary>
		/// 連線master的timeout秒數
		/// </summary>
		public readonly int CONNECTSECS = 3;

		/// <summary>
		/// ServerId
		/// </summary>
		public readonly Guid ServerId = Guid.NewGuid();

		/// <summary>
		/// Server類型
		/// </summary>
		public abstract ServerTypeEnum ServerType { get; }
		/// <summary>
		/// 子Server port
		/// </summary>
		public abstract int SubServerPort { get; }
		/// <summary>
		/// Client port
		/// </summary>
		public abstract int ClientPort { get; }
		/// <summary>
		/// 是否連線到master
		/// </summary>
		public abstract bool ConnectsToMaster { get; }
		/// <summary>
		/// master的端點位址
		/// </summary>
		public abstract IPEndPoint MasterEndPoint { get; }
		/// <summary>
		/// 重連master的秒數
		/// </summary>
		public abstract int ConnectRetryIntervalSeconds { get; }

		/// <summary>
		/// master session
		/// </summary>
		public GameSession MasterSession { get; set; }
		/// <summary>
		/// Server狀態
		/// </summary>
		public ServerStateEnum ServerState { get; set; }
		/// <summary>
		/// 連線管理
		/// </summary>
		public ConnectionCollection Connections { get; set; }

		/// <summary>
		/// 遊戲Logger
		/// </summary>
		public CommonLib.Utility.Logger GameLogger { get; set; }
		/// <summary>
		/// 重連master的計時器
		/// </summary>
		private Timer m_retry;

		public GameServer()
			: base(new DefaultReceiveFilterFactory<GameMessageFilter, GameMessage>())
		{
			ServerState = ServerStateEnum.Idle;
			CreateConnectionCollection();
			GameLogger = new GameLogger(log4net.LogManager.GetLogger(GetType().Name));
		}
		protected virtual void CreateConnectionCollection()
		{
			Connections = new ConnectionCollection(this);
		}

		public override bool Start()
		{
			if (ServerState == ServerStateEnum.Running)
			{
				return false;
			}
			ServerConfig config = new ServerConfig();
			config.Name = GetType().Name;
			config.MaxRequestLength = 32768;
			config.MaxConnectionNumber = 10000;
			config.DisableSessionSnapshot = true;

			List<ListenerConfig> listeners = new List<ListenerConfig>();
			if (SubServerPort != -1)
			{
				listeners.Add(
					new ListenerConfig()
					{
						Ip = "Any",
						Port = SubServerPort
					});
				listeners.Add(
					new ListenerConfig()
					{
						Ip = "IPv6Any",
						Port = SubServerPort
					});
			}
			if (ClientPort != -1)
			{
				listeners.Add(
					new ListenerConfig()
					{
						Ip = "Any",
						Port = ClientPort
					});
				listeners.Add(
					new ListenerConfig()
					{
						Ip = "IPv6Any",
						Port = ClientPort
					});
			}
			config.Listeners = listeners;
			List<CommandAssemblyConfig> commandAssemblies = new List<CommandAssemblyConfig>();
			commandAssemblies.Add(
				new CommandAssemblyConfig()
				{
					Assembly = typeof(GameServer).Assembly.FullName
				});
			config.CommandAssemblies = commandAssemblies;

			if (Setup(config) == false)
			{
				GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error, "Server setups failed!");
				return false;
			}
			if (base.Start() == false)
			{
				GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error, "Server starts failed!");
				return false;
			}
			ServerState = ServerStateEnum.Running;
			return true;
		}
		/// <summary>
		/// 停止
		/// </summary>
		public override void Stop()
		{
			if (ServerState == ServerStateEnum.Idle)
			{
				return;
			}
			base.Stop();
			ServerState = ServerStateEnum.Idle;
		}
		protected override void OnStarted()
		{
			base.OnStarted();

			// 連線master
			if (ConnectsToMaster)
			{
				ConnectToMaster();
			}
		}
		protected override void OnNewSessionConnected(GameSession session)
		{
			base.OnNewSessionConnected(session);

			if (session == null)
			{
				return;
			}
			// 向master註冊
			if (session.RemoteEndPoint.Equals(MasterEndPoint) == true)
			{
				Register(session);
			}
		}
		protected override void OnSessionClosed(GameSession session, CloseReason reason)
		{
			base.OnSessionClosed(session, reason);

			if (MasterSession == session)
			{
				MasterSession = null;
				Connections.DisconnectAll();
				OnMasterDisconnect();
			}
			else if (IsSubServer(session) == true)
			{
				Connections.OnSubServerDisconnect(session);
			}
			else if (IsClient(session) == true)
			{
				Connections.OnClientDisconnect(session);
			}
		}

		/// <summary>
		/// 連線master
		/// </summary>
		public void ConnectToMaster()
		{
			try
			{
				var activeConnector = this as IActiveConnector;
				var task = activeConnector.ActiveConnect(MasterEndPoint);
				TimeSpan ts = TimeSpan.FromSeconds(CONNECTSECS);
				if (task.Wait(ts) == false)
				{
					GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error, "Connects to master failed!");
					ReconnectToMaster();
				}
			}
			catch (Exception e)
			{
				GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error, "Connects to master failed! exception = {0}", e.Message);
			}
		}
		/// <summary>
		/// 重連master
		/// </summary>
		public void ReconnectToMaster()
		{
			m_retry = new Timer(o => ConnectToMaster(), null, ConnectRetryIntervalSeconds * 1000, 0);
		}
		/// <summary>
		/// 對master註冊
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public virtual void Register(GameSession session)
		{

		}
		/// <summary>
		/// 對master註冊完成
		/// </summary>
		public virtual void OnRegistered()
		{

		}
		/// <summary>
		/// master斷線觸發
		/// </summary>
		public virtual void OnMasterDisconnect()
		{

		}
		/// <summary>
		/// 是否為子Server
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public bool IsSubServer(GameSession session)
		{
			if (SubServerPort == -1)
			{
				return false;
			}
			if (session.LocalEndPoint.Port == SubServerPort)
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// 是否為Client
		/// </summary>
		/// <param name="session">遊戲Session</param>
		public bool IsClient(GameSession session)
		{
			if (ClientPort == -1)
			{
				return false;
			}
			if (session.LocalEndPoint.Port == ClientPort)
			{
				return true;
			}
			return false;
		}
	}
}
