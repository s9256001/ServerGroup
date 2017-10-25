using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CacheServer.Models
{
	[Table("Player")]
	public class PlayerModel
	{
		[Key]
		public int PlayerId { get; set; }
		public DateTime LoginTime { get; set; }
		public DateTime LogoutTime { get; set; }
	}
}
