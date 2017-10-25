using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GameCommon
{
	public class PlayerData
	{
		[JsonIgnore]
		public const int MaxAccountLength = 30;
		[JsonIgnore]
		public const int MinAccountLength = 6;
		[JsonIgnore]
		public const int MaxPasswordLength = 30;
		[JsonIgnore]
		public const int MinPasswordLength = 6;
		[JsonIgnore]
		public const int MaxRoleNum = 6;
		[JsonIgnore]
		public const int MaxRoleNameLength = 20;

		public int PlayerId { get; set; }
		public Guid PlayerKey { get; set; }
		public DateTime LoginTime { get; set; }
		public DateTime LogoutTime { get; set; }
		public List<RoleData> RoleDatas { get; set; }
	}
}
