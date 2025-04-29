//!=>Referencia [1] [Sección 10, 045. Crear dependencias en proyecto Application]

using CleanArchitecture.Application.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.Application
{
	public static class ApplicationServiceRegistration
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			//! [1] AutoMapper 13 NO REQUIERE PAQUETE NUGET: AutoMapper.Extensions.Microsoft.DependencyInjection
			//! [1] DEBE REGISTRARSE EL ASSEMBLY DE AUTOMAPPER: automáticamente va a leer TODAS las clases que estén imlementando las interfaces de AutoMapper y las va a inyectar
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			//! [1] BUSCA TODAS LAS CLASES DEL PROYECTO QUE REFERENCÍEN al AbstractValidation y a los paquetes de FluentValitadion y automáticamente va a inyectar (instanciar -crear los objetos-) para que esa validación sea posible dentro del proyecto
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddMediatR(Assembly.GetExecutingAssembly());

			services.AddTransient(
				typeof(IPipelineBehavior<,>),
				typeof(UnhandledExceptionBehaviour<,>)
			);

			services.AddTransient(
				typeof(IPipelineBehavior<,>),
				typeof(ValidationBehaviour<,>)
			);

			return services;
		}
	}
}
