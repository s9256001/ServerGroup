using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CacheServer.Models
{
	[Table("Account")]
	public class AccountModel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PlayerId { get; set; }
		[MaxLength(32)]
		public string Account { get; set; }
		[MaxLength(32)]
		public string Password { get; set; }
		public DateTime CreateTime { get; set; }
	}
}
