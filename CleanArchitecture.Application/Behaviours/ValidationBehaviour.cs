//!=>Referencia [1] [Sección 08, 043. MediatR y Behaviours]

using FluentValidation;
using MediatR;
//! [1] DENTRO DE FluentValidation YA EXISTE UN ValidationException, PARA UTIILZAR EL PERSONALIZADO POR NOSOTROS USAMOS UN "ALIAS"
using ValidationException = CleanArchitecture.Application.Exceptions.ValidationException;

namespace CleanArchitecture.Application.Behaviours
{
	/*
	 * Si se está utlizando una versión de MediatR 10 en adelante al no agregar:
	 *		where TRequest : IRequest<TResponse>
	 * aparecerá un error. Esto sucede porque se tiene que definir el tipo de dato para el TRequest
	 */
	public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;

		public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
		{
			_validators = validators;
		}

		// El objetivo de "ValidationBehaviour" es capturar el objeto Request que está llegando desde el cliente y evaluar si existe alguna validación de alguna de las propiedades que envía el cliente. Entonces se tiene que hacer un "match" entre lo que envía el cliente VS las validaciones que yo he escrito en el interior de mi aplicación
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			//! [1] VALIDAMOS QUE SE TENGA UNA VALIDACIÓN LÓGICA ESCRITA EN LA APLICACIÓN
			if (_validators.Any())
			{
				//! Si se tiene alguna validación, entonces, a continuación se hará la evaluación de cada validación VS la propiedad del objeto request que envía el cliente

				//! [1] EL CONTEXT DEBE SER DE TIPO TRequest Y PASAMOS EL REQUEST ENVIADO POR EL CLIENTE
				var context = new ValidationContext<TRequest>(request);

				//! [1] BUSCA Y EJECUTA (DESDE EL PIPELINE) TODAS LAS VALIDACIONES DENTRO DE LA APLICACIÓN
				var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

				//! [1] GENERA UN LISTADO DE ERRORES DE VALIDACIÓN
				var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

				if (failures.Count != 0)
				{
					//! [1] EJECUTA LA VALIDACIÓN PERSONALIZADA QUE MUESTRA CON MÁS DETALLE LOS ERRORES
					throw new ValidationException(failures);
				}
			}

			return await next();
		}
	}
}
