using MediatR;

namespace CleanArchitecture.Application.Features.Streamers.Commands.DeleteStreammer
{
	public class DeleteStreamerCommand : IRequest
	{
        public int Id { get; set; }
    }
}
