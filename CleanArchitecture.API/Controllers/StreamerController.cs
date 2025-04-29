using CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer;
using CleanArchitecture.Application.Features.Streamers.Commands.DeleteStreammer;
using CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class StreamerController : ControllerBase
	{
		private readonly IMediator _mediator;

		public StreamerController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost(Name = "CreateStreamer")]
		[Authorize(Roles = "Administrator")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public async Task<ActionResult<int>> CreateStreamer([FromBody]CreateStreamerCommand command)
		{
			return await _mediator.Send(command);
		}

		// Este endpoint es muy probable que devuelva un resultado como "No encontrado" y también es probable que devuelva un resultado como "No contenido" (que no tiene un contenido en el body de response)
		[HttpPut(Name = "UpdateStreamer")]
		// Significa que envía un response sin ningún contenido
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		// Significa que NO encontró el objeto a actualizar
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> UpdateStreamer([FromBody]UpdateStreamerCommand command)
		{
			await _mediator.Send(command);

			return NoContent();
		}

		[HttpDelete("{id}", Name = "DeleteStreamer")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> DeleteStreamer(int id)
		{
			var command = new DeleteStreamerCommand()
			{
				Id = id
			};
			await _mediator.Send(command);

			return NoContent();
		}
	}
}
