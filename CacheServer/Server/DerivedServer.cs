using GameCommon;
using ServerCommon;
using System;
using System.Collections.Generic;
using System.Net;

namespace CacheServer.Server
{
	public class DerivedServer : GameServer
	{
		public readonly string PublicServerAddress = Settings.Default.PublicServerAddress;

		public override ServerTypeEnum ServerType
		{
			get { return ServerTypeEnum.Cache; }
		}
		public override int SubServerPort
		{
			get { return Settings.Default.SubServerPort; }
		}
		public override int ClientPort
		{
			get { return -1; }
		}
		public override bool ConnectsToMaster
		{
			get { return false; }
		}
		public override IPEndPoint MasterEndPoint
		{
			get { return null; }
		}
		public override int ConnectRetryIntervalSeconds
		{
			get { return 0; }
		}

		public Dictionary<Guid, PlayerData> PlayerDatas { get; set; }

		public DerivedServer()
			: base()
		{
			PlayerDatas = new Dictionary<Guid, PlayerData>();
		}
	}
}
