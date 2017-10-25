using GameCommon;
using Newtonsoft.Json;
using ServerCommon;

namespace RegionServer.Data
{
	public class DerivedPlayerData : PlayerData
	{
		[JsonIgnore]
		public GameSession Session { get; set; }
	}
}
