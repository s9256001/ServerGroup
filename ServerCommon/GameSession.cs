using CommonLib.Network;
using GameCommon;
using SuperSocket.SocketBase;
using System;

namespace ServerCommon
{
	/// <summary>
	/// 遊戲Session
	/// </summary>
	public class GameSession : AppSession<GameSession, GameMessage>
	{
		/// <summary>
		///  Server類型
		/// </summary>
		public ServerTypeEnum ServerType { get; set; }
		/// <summary>
		/// PeerId
		/// </summary>
		public Guid PeerId { get; set; }
		/// <summary>
		/// Client port
		/// </summary>
		public int ClientPort { get; set; }

		/// <summary>
		/// 遊戲Logger
		/// </summary>
		private CommonLib.Utility.Logger GameLogger { get; set; }
		/// <summary>
		/// 封包打包解包處理
		/// </summary>
		private Packer Packer { get; set; }

		public GameSession() : base()
		{
			GameLogger = new GameLogger(log4net.LogManager.GetLogger(GetType().Name));
			Packer = new GamePacker(GameLogger);
		}

		/// <summary>
		/// 傳送封包
		/// </summary>
		/// <param name="packet">封包</param>
		public bool SendPacket(CommonBasePacket packet)
		{
			GameServer Server = AppServer as GameServer;
			string message = Newtonsoft.Json.JsonConvert.SerializeObject(packet);
			byte[] data = Packer.WritePacket(message);
			return TrySend(data, 0, data.Length);
		}
	}
}
