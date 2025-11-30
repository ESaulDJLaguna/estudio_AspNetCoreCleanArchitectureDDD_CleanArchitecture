//!=>Referencia [1] [Sección 08, 037. Command Handler y CQRS]

using AutoMapper;
using CleanArchitecture.Application.Contracts.Infrastructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer
{
    //! [1] HEREDAMOS DE IRequestHandler, E INDICAMOS LOS PARÁMETROS QUE VAN A INGRESAR Y LOS QUE VA A DEVOLVER
    public class CreateStreamerCommandHandler : IRequestHandler<CreateStreamerCommand, int>
    {
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        //! [1] DEBEMOS INDICAR SOBRE QUÉ CLASE VA A TRABAJAR
        private readonly ILogger<CreateStreamerCommandHandler> _logger;
        private EmailSettings _emailSettings { get; }

        public CreateStreamerCommandHandler(IStreamerRepository streamerRepository, IMapper mapper, IEmailService emailService, ILogger<CreateStreamerCommandHandler> logger, IUnitOfWork unitOfWork, IOptions<EmailSettings> emailSettings)
        {
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
            _emailSettings = emailSettings.Value;
        }

        //! [1] IMPLEMENTACIÓN DE LA INTERFAZ
        public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
        {
            var streamerEntity = _mapper.Map<Streamer>(request);
            //var newStreamer = await _streamerRepository.AddAsync(streamerEntity);

            _unitOfWork.StreamerRepository.AddEntity(streamerEntity);
            var result = await _unitOfWork.Complete();

            if(result <= 0)
            {
                throw new Exception($"No se pudo insertar el record del streamer");
            }

            //_logger.LogInformation($"Streamer {newStreamer.Id} fue creado exitosamente");
            _logger.LogInformation($"Streamer {streamerEntity.Id} fue creado exitosamente");

            //await SendEmail(newStreamer);
            await SendEmail(streamerEntity);

			//return newStreamer.Id;
			return streamerEntity.Id;
		}

        private async Task SendEmail(Streamer streamer)
        {
            var email = new Email
            {
                //TODO: AGREGAR CORREO ELECTRÓNICO
                To = _emailSettings.ToAddress,
                Body = "<p style='color: red;'>La compañía de streamer se creó correctamente</p>",
                Subject = "Mensaje de prueba CleanArchitecture"
            };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errores enviando el email de {streamer.Id}");
            }
        }
    }
}
