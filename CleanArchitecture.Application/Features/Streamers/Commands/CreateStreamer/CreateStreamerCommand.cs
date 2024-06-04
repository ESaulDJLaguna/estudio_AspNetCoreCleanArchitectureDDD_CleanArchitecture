//!=>Referencia [1] [Sección 08, 037. Command Handler y CQRS]

using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer
{
    //[1] SE QUIERE QUE EL REQUEST DEVUELVA EL ID DEL RECORD GENERADO (DEVUELVE UN int)
    public class CreateStreamerCommand : IRequest<int>
    {
        public string Nombre { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
