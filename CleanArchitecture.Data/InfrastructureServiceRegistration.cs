using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Infrastructure.Email;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace CleanArchitecture.Infrastructure
{
	public static class InfrastructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			// Cadena de conexión hacía la base de datos
			services.AddDbContext<StreamerDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));

			services.AddScoped<IVideoRepository, VideoRepository>();
			services.AddScoped<IStreamerRepository, StreamerRepository>();

			// Para la versión .NET 6 se requiere instalar el paquete: Microsoft.Extensions.Options.ConfigurationExtensions
			// de lo contrario los valores estarán en null
			// Se podría realizar de las siguientes tres formas:
			// 1) services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
			// 2) services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
			// 3) 
			services.Configure<EmailSettings>(configuration.GetSection(typeof(EmailSettings).Name));
			
			services.AddTransient<IEmailService, GmailService>();

			return services;
		}
	}
}
