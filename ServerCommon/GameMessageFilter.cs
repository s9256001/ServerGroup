using System;
using System.Text;
using SuperSocket.Facility.Protocol;
using CommonLib.Network;
using GameCommon;

namespace ServerCommon
{
	/// <summary>
	/// 遊戲訊息過濾器
	/// header: body大小
	/// 將body轉為GameMessage
	/// </summary>
	public class GameMessageFilter : FixedHeaderReceiveFilter<GameMessage>
	{
		/// <summary>
		/// 遊戲Logger
		/// </summary>
		public CommonLib.Utility.Logger GameLogger { get; private set; }

		/// <summary>
		/// 封包類型字串 + COMMAND_NAME組成Command的名稱
		/// </summary>
		private const string COMMAND_NAME = "Command";
		/// <summary>
		/// 封包打包解包處理
		/// </summary>
		private Packer m_packer = null;

		public GameMessageFilter()
			: base(4)// header為4 bytes
		{
			GameLogger = new GameLogger(log4net.LogManager.GetLogger(GetType().Name));
			m_packer = new GamePacker(GameLogger);
		}

		protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
		{
			byte[] bytes = new byte[Size];
			for (int i = 0; i < Size; ++i)
			{
				bytes[i] = header[offset + i];
			}
			int leng = BitConverter.ToInt32(bytes, 0);
			return leng;
		}
		protected override GameMessage ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
		{
			GameMessage res = new GameMessage();

			byte[] buffer = new byte[length];
			Array.Copy(bodyBuffer, offset, buffer, 0, length);

			string message = Encoding.UTF8.GetString(buffer);

			GameBasePacket packet = null;
			try
			{
				packet = m_packer.DerivePacket(message) as GameBasePacket;
			}
			catch
			{
				packet = null;
			}
			if (packet == null)
			{
				LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Error, "Derive packet failed! message = {0}", message);
				return res;
			}
			string packetTypeName = m_packer.GetPacketTypeName(packet.m_type);
			res.Key = packetTypeName + COMMAND_NAME;
			res.Packet = packet;

			LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Debug,
				"Receive type = {0}, length = {1}, message = {2}",
				packetTypeName,
				message.Length,
				message.Substring(0, Math.Min(500, message.Length)) +
				(message.Length > 500 ? " ..." : ""));
			return res;
		}

		/// <summary>
		/// Log訊息
		/// </summary>
		/// <param name="type">Log類型</param>
		/// <param name="text">訊息格式</param>
		/// <param name="args">訊息格式化參數</param>
		private void LogMessage(CommonLib.Utility.Logger.LogTypeEnum type, string text, params object[] args)
		{
			if (GameLogger == null)
			{
				return;
			}
			GameLogger.LogMessage(type, text, args);
		}
	}
}
