using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.DeleteStreammer
{
	public class DeleteStreamerCommandHandler : IRequestHandler<DeleteStreamerCommand>
	{
		private readonly IStreamerRepository _streamerRepository;
		private readonly ILogger<DeleteStreamerCommandHandler> _logger;

		public DeleteStreamerCommandHandler(IStreamerRepository streamerRepository, ILogger<DeleteStreamerCommandHandler> logger)
		{
			_streamerRepository = streamerRepository;
			_logger = logger;
		}

		public async Task<Unit> Handle(DeleteStreamerCommand request, CancellationToken cancellationToken)
		{
			var streamerToDelete = await _streamerRepository.GetByIdAsync(request.Id);

			if (streamerToDelete is null)
			{
				_logger.LogError($"{request.Id} streamer no existe en el sistema");
				throw new NotFoundException(nameof(Streamer), request.Id);
			}

			await _streamerRepository.DeleteAsync(streamerToDelete);

			_logger.LogInformation($"El {request.Id} streamer fue eliminado con exito");

			return Unit.Value;
		}
	}
}
