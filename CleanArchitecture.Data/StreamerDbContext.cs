using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Data
{
	public class StreamerDbContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// El @ se utiliza para que reconozca el caracter especial \
			optionsBuilder.UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=Streamer;Integrated Security=True")
				.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name}, LogLevel.Information)
				.EnableSensitiveDataLogging();
		}

		public DbSet<Streamer>? Streamers { get; set; }
		public DbSet<Video>? Videos { get; set; }
	}
}
