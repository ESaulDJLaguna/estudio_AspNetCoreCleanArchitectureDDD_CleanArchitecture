using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Behaviours
{
	/*
	 * Se creará un nuevo Behavior. Lo que hará es monitorear las operaciones dentro del Handle, es decir, si ocurriera algún error lógico, por ejemplo dentro de la creación de un nuevo streamer (en la clase CreateStreamerCommandHandler), entonces quiero que se ejecute un log de error y también quiero que se dispare una nueva excepción
	 */
	public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
	{
		private readonly ILogger<TRequest> _logger;

		public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
		{
			_logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			try
			{
				return await next();
			}
			catch (Exception ex)
			{
				var requestName = typeof(TRequest).Name;

				_logger.LogError(ex, "Application Request: Sucedió una excepción para el request {Name} {@Request}", requestName, request);
				throw;
			}
		}
	}
}
