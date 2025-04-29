//!=>Referencia [1] [Sección 01, 007 . Inserción de record en EFC]
//!=>Referencia [2] [Sección 01, 013. Fluent API]
//!=>Referencia [3] [Sección 01, 014. Muchos a muchos en EF]

using CleanArchitecture.Domain;
using CleanArchitecture.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Persistence
{
	public class StreamerDbContext : DbContext
	{
		// Otro componente de mi solución va a ser el encargado de setear la cadena de conexión en esta instancia de EF llamada StreamerDbContext
		public StreamerDbContext(DbContextOptions<StreamerDbContext> options) : base(options) { }

		//! Este método se va a ejecutar justamente antes de insertar el nuevo record dentro de la base de datos o antes de actualizar el record en la base de datos.
		//! Se utilizará para "llenar" las propiedades "comunes", es decir, las que son heredadas de: BaseDomainModel
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			//! Se le indicará que recorra todas las entidades antes de realizar la operación
			foreach (var entry in ChangeTracker.Entries<BaseDomainModel>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						entry.Entity.CreatedDate = DateTime.Now;
						entry.Entity.CreatedBy = "system";
						break;
					case EntityState.Modified:
						entry.Entity.LastModifiedDate = DateTime.Now;
						entry.Entity.LastModifiedBy = "system";
						break;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}

		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//    // El @ se utiliza para que reconozca el caracter especial \
		//    optionsBuilder
		//        .UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=Streamer;Integrated Security=True")
		//        //! [1] REGISTRA TODOS LOS EVENTOS EN LAS CATEGORÍAS ESPECIFICADAS
		//        .LogTo(
		//            //! [1] IMPRIME EN CONSOLA LOS QUERIES
		//            Console.WriteLine,
		//            //! [1] ARREGLO QUE REPRESENTA EL DEBUGGER CATEGORY
		//            new[] { DbLoggerCategory.Database.Command.Name },
		//            //! [1] NIVEL DE LOG A CAPTURAR
		//            LogLevel.Information
		//        )
		//        //! [1] PERMITE DESCRIBIR CADA UNA DE LAS OPERACIONES
		//        .EnableSensitiveDataLogging();
		//}

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
				.UsingEntity<VideoActor>(pt =>
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
