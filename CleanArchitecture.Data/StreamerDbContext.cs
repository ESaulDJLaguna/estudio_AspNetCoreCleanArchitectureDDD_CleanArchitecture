//!=>Referencia [1] [Sección 01, 007 . Inserción de record en EFC]
//!=>Referencia [2] [Sección 01, 013. Fluent API]
//!=>Referencia [3] [Sección 01, 014. Muchos a muchos en EF]

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
			optionsBuilder
				.UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=Streamer;Integrated Security=True")
				//! [1] REGISTRA TODOS LOS EVENTOS EN LAS CATEGORÍAS ESPECIFICADAS
				.LogTo(
					//! [1] IMPRIME EN CONSOLA LOS QUERIES
					Console.WriteLine,
					//! [1] ARREGLO QUE REPRESENTA EL DEBUGGER CATEGORY
					new[] { DbLoggerCategory.Database.Command.Name },
					//! [1] NIVEL DE LOG A CAPTURAR
					LogLevel.Information
				)
				//! [1] PERMITE DESCRIBIR CADA UNA DE LAS OPERACIONES
				.EnableSensitiveDataLogging();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			//![2] Dentro de Entity se indica la propiedad que se quiere evaluar
			modelBuilder.Entity<Streamer>()
				//![2] UN Streamer VA A TENER MUCHAS INSTANCIAS DE Videos
				.HasMany(m => m.Videos)
				//![2] CON UN ELEMENTO Streamer
				.WithOne(m => m.Streamer)
				//![2] INDICAMOS CUÁL SERÁ LA LLAVE FORÁNEA
				.HasForeignKey(m => m.StreamerId)
				//![2] INDICAMOS QUE ES REQUERIDO
				.IsRequired()
				//![2] ESTABLECEMOS QUE TENDRÁ UNA ELIMINACIÓN EN CASCADA
				.OnDelete(DeleteBehavior.Restrict);


			//![3] SE COMENZARÁ EVALUANDO A VÍDEO
			modelBuilder.Entity<Video>()
				//![3] VA A TENER MUCHAS INSTANCIAS DE LA CLASE ACTORES
				.HasMany(p => p.Actores)
				//![3] CON UNA RELACIÓN DE MUCHOS VIDEOS
				.WithMany(t => t.Videos)
				//![3] QUÉ ENTIDAD ESTÁ UTILIZANDO PARA DEFINIR LA RELACIÓN
				.UsingEntity<VideoActor>( pt =>
					//![3] SE DEFINEN LAS PROPIEDADES QUE REPRESENTARÁN LA CLAVE PRIMARIA COMPUESTA DE ESTA ENTIDAD VideoActor QUE SON DOS
					pt.HasKey(e => new { e.ActorId, e.VideoId })
				);
		}

		public DbSet<Streamer>? Streamers { get; set; }
		public DbSet<Video>? Videos { get; set; }
		public DbSet<Actor>? Actores { get; set; }
		public DbSet<Director>? Directores { get; set; }
	}
}
