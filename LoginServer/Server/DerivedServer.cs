using GameCommon;
using ServerCommon;
using System.Net;

namespace LoginServer.Server
{
	public class DerivedServer : SubServerBase
	{
		private readonly IPEndPoint m_masterEndPoint = new IPEndPoint(IPAddress.Parse(Settings.Default.MasterIP), Settings.Default.MasterPort);

		public override ServerTypeEnum ServerType
		{
			get { return ServerTypeEnum.Login; }
		}
		public override int SubServerPort
		{
			get { return -1; }
		}
		public override int ClientPort
		{
			get { return Settings.Default.ClientPort; }
		}
		public override bool ConnectsToMaster
		{
			get { return true; }
		}
		public override IPEndPoint MasterEndPoint
		{
			get { return m_masterEndPoint; }
		}
		public override int ConnectRetryIntervalSeconds
		{
			get { return Settings.Default.ConnectRetryIntervalSeconds; }
		}

		public override void OnRegistered()
		{
			GameLogger.LogMessage(CommonLib.Utility.Logger.LogTypeEnum.Info, "OnRegistered!");
		}
	}
}
