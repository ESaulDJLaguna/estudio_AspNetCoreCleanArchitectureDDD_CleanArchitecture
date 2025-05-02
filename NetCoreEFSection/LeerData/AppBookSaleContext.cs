using LeerData.Models;
using Microsoft.EntityFrameworkCore;

namespace LeerData
{
	public class AppBookSaleContext : DbContext
	{
		private const string CONNECTION_STRING = @"Data Source=localhost\SQLEXPRESS; Initial Catalog=BooksWeb; Integrated Security=True; TrustServerCertificate=True";

		//! Este método será el encargado de crear la instancia hacia el servidor SQL Server, al cual queremos conectarnos
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(CONNECTION_STRING);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			/*
			 * Indicamos que la entidad AuthorBook tiene DOS claves primarias.
			 * 
			 * Esto es MUY IMPORTANTE de agregar cada vez que se tenga más de una llave primaria en una entidad.
			 */
			modelBuilder.Entity<AuthorBook>().HasKey(xi => new { xi.AuthorId, xi.BookId });
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<Price> Prices { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<AuthorBook> AuthorsBooks { get; set; }
	}
}