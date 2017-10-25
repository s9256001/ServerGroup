using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CacheServer.Models
{
	[Table("Role")]
	public class RoleModel
	{
		[Key]
		public Guid RoleId { get; set; }
		[MaxLength(32)]
		public string Name { get; set; }
		public byte[] Image { get; set; }
		public DateTime CreateTime { get; set; }
		public int PlayerId { get; set; }
	}
}
