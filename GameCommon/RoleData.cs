using System;
using System.Collections.Generic;

namespace GameCommon
{
	public class RoleData
	{
		public Guid RoleId { get; set; }
		public string Name { get; set; }
		public List<byte> Image { get; set; }
		public DateTime CreateTime { get; set; }
		public int PlayerId { get; set; }
	}
}
