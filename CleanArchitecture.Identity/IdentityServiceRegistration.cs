using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Models.Identity;
using CleanArchitecture.Identity.Models;
using CleanArchitecture.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanArchitecture.Identity
{
	public static class IdentityServiceRegistration
	{
		public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));

			services.AddDbContext<CleanArchitectureIdentityDbContext>(options =>
				options.UseSqlServer(
					configuration.GetConnectionString("IdentityConnectionString")!,
					b => b.MigrationsAssembly(typeof(CleanArchitectureIdentityDbContext).Assembly.FullName)
				));

			// Se agrega la instancia para ApplicationUser
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<CleanArchitectureIdentityDbContext>()
				.AddDefaultTokenProviders();

			services.AddTransient<IAuthService, AuthService>();
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					ValidIssuer = configuration["JwtSettings:Issuer"], // Quién está enviando (generando) el token
					ValidAudience = configuration["JwtSettings:Audience"], // A quién va dirigido
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)), // Palabra clave (reservada), sobre el cual gira el algoritmo para saber si el token es legitimo o no
				};
			});

			return services;
		}
	}
}
