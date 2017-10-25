using System.Data.Entity;

namespace CacheServer.Models
{
	public class GameDbContext : DbContext
	{
		public DbSet<AccountModel> Accounts { get; set; }
		public DbSet<PlayerModel> Players { get; set; }
		public DbSet<RoleModel> Roles { get; set; }
	}
}
