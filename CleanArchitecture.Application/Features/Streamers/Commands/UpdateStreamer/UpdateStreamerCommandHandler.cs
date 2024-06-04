//!=>Referencia [1] [Sección 08, 040. Update Command]

using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer
{
	internal class UpdateStreamerCommandHandler : IRequestHandler<UpdateStreamerCommand>
	{
		private readonly IStreamerRepository _streamerRepository;
		private readonly IMapper _mapper;
		private readonly ILogger<UpdateStreamerCommandHandler> _logger;

		public UpdateStreamerCommandHandler(IStreamerRepository streamerRepository, IMapper mapper, ILogger<UpdateStreamerCommandHandler> logger)
		{
			_streamerRepository = streamerRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<Unit> Handle(UpdateStreamerCommand request, CancellationToken cancellationToken)
		{
			var streamerToUpdate = await _streamerRepository.GetByIdAsync(request.Id);

			if (streamerToUpdate is null)
			{
				_logger.LogError($"No se encontró el streamer id {request.Id}");
				throw new NotFoundException(nameof(Streamer), request.Id);
			}

			//! [1] LOS DATOS ENVIADOS POR EL CLIENTE (request) SE COPIAN A streamerToUpdate
			_mapper.Map(request, streamerToUpdate, typeof(UpdateStreamerCommand), typeof(Streamer));

			await _streamerRepository.UpdateAync(streamerToUpdate);

			_logger.LogInformation($"La operación fue exitosa actualizando el streamer {request.Id}");

			//! [1] DEVUELVE COMO RESULTADO UN VALOR ENTERO PARA SABER SI FUE EXITOSO O NO
			return Unit.Value;
		}
	}
}
