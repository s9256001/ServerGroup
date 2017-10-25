using CommonLib.Network;
using CommonLib.Utility;
using System;

namespace GameCommon
{
	public class GamePacker : Packer
	{
		public GamePacker(Logger logger) : base(logger)
		{
			
		}

		public override Type GetPacketTypeInfo()
		{
			return typeof(GamePacketType);
		}
	}
}
