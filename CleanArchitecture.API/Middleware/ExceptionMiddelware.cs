using CleanArchitecture.API.Errors;
using CleanArchitecture.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace CleanArchitecture.API.Middleware
{
	public class ExceptionMiddelware
	{
		// Representa el pipeline que va a continuar hacia la siguiente fase en caso de que NO ocurra ninguna excepción
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddelware> _logger;
		// Se utiliza para saber si la aplicación se está ejecutando en producción o en un ambiente de desarrollo
		private readonly IHostEnvironment _env;

		public ExceptionMiddelware(RequestDelegate next, ILogger<ExceptionMiddelware> logger, IHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Evaluación del request que envía el cliente y en caso de que dicho request tenga errores, se trabajará con una Exception
			try
			{
				// Permite que el request pase al siguiente nivel hasta procesarse en la aplicación
				await _next(context);
			}
			catch (Exception ex)
			{
				// En caso de que existieran errores, de validación o alguna lógica de negocio, se va a querer imprimir el detalle de ese error
				_logger.LogError(ex, ex.Message);

				// Ahora se va a querer crear un objeto response hacia el cliente con detalles de este error para que sepan en qué se equivocaron
				context.Response.ContentType = "application/json";
				//context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				var statusCode = (int)HttpStatusCode.InternalServerError;
				// Representa el mensaje de detalle de la excepción
				var result = string.Empty;

				// Personalización de la excepción
				switch (ex)
				{
					case NotFoundException notFoundException:
						statusCode = (int)HttpStatusCode.NotFound;
						break;

					case ValidationException validationException:
						statusCode = (int)HttpStatusCode.BadRequest;
						var validationJson = JsonConvert.SerializeObject(validationException.Errors);
						result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, ex.Message, validationJson));
						break;

					case BadRequestException badRequestException:
						statusCode = (int)HttpStatusCode.BadRequest;
						break;

					default:
						break;
				}

				if (string.IsNullOrEmpty(result))
				{
					result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, ex.Message, ex.StackTrace));
				}

				context.Response.StatusCode = statusCode;

				/*
				// Se quiere que el mensaje de respuesta que se va a enviar al cliente dependa de si el ambiente es productivo o desarrollo (en este caso queremos que se envíe mucha información con respecto del error, para como desarrollador saber en qué se está equivando)
				var response = _env.IsDevelopment()
					? new CodeErrorException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
					: new CodeErrorException((int)HttpStatusCode.InternalServerError);

				// Se tiene que enviar la data en un formato tipo Json
				var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

				var json = System.Text.Json.JsonSerializer.Serialize(response, options);

				// Envia el mensaje Json hacia el cliente
				await context.Response.WriteAsync(json);
				*/
				await context.Response.WriteAsync(result);
			}
		}
	}
}
