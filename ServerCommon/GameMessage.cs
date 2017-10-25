using GameCommon;
using SuperSocket.SocketBase.Protocol;

namespace ServerCommon
{
	/// <summary>
	/// 遊戲訊息
	/// </summary>
	public class GameMessage : IRequestInfo
	{
		/// <summary>
		/// Key
		/// </summary>
		public string Key { get; set; }
		/// <summary>
		/// 封包
		/// </summary>
		public GameBasePacket Packet { get; set; }
	}
}
