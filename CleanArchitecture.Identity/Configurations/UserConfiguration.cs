using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
	// Aquí se agregará la data de los usuarios que quiero crear por defecto
	public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			var hasher = new PasswordHasher<ApplicationUser>();
			builder.HasData(
				new ApplicationUser
				{
					// Página para generar Guids en línea: https://guidgenerator.com/
					Id = "c2f25781-772a-4038-a598-970d4bb39363",
					Email = "admin@localhost.com",
					NormalizedEmail = "admin@localhost.com",
					Nombre = "Admin",
					Apellidos = "User",
					UserName = "admin@localhost.com",
					NormalizedUserName = "admin@localhost.com",
					PasswordHash = hasher.HashPassword(null, "VaxiDrez2025$"),
					EmailConfirmed = true
				},
				new ApplicationUser
				{
					Id = "3747618c-0a63-4c29-bd58-1fc91a4257fa",
					Email = "juanperez@localhost.com",
					NormalizedEmail = "juanperez@localhost.com",
					Nombre = "Juan",
					Apellidos = "Pérez",
					UserName = "juanperez@localhost.com",
					NormalizedUserName = "juanperez@localhost.com",
					PasswordHash = hasher.HashPassword(null, "VaxiDrez2025$"),
					EmailConfirmed = true
				}
			);
		}
	}
}
