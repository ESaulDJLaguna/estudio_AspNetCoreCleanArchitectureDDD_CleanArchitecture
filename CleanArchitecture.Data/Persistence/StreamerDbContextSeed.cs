using CleanArchitecture.Domain;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence
{
	/*
	 * Esta clase se utilizará para agregar data de prueba en las tablas solo si están vacías.
	 * Seed significa "alimentar", por lo que "alimentaremos" de data a nuestra base de datos
	 */
	public class StreamerDbContextSeed
	{
		public static async Task SeedAsync(StreamerDbContext context, ILogger<StreamerDbContextSeed> logger)
		{
			if(!context.Streamers.Any())
			{
				context.Streamers.AddRange(GetPreConfiguredStreamer());
				await context.SaveChangesAsync();
				logger.LogInformation("Estamos insertando nuevos records al db {context}", typeof(StreamerDbContext).Name);
			}

		}

		private static IEnumerable<Streamer> GetPreConfiguredStreamer()
		{
			// Primero se va a ejecutar el método SaveChangesAsync() dentro de StreamerDbContext, por lo que se establecerá en primer lugar el CreatedBy = "system", pero a continuación se llenará la data de prueba y se "sobreescribirá" el CreatedBy
			return new List<Streamer>()
			{
				new Streamer { CreatedBy = "ESaulDJLaguna", Nombre = "Maxi HBP", Url = "http://hbp.com" },
				new Streamer { CreatedBy = "ESaulDJLaguna", Nombre = "Amazon VIP", Url = "http://amazonvip.com" }
			};
		}
	}
}
