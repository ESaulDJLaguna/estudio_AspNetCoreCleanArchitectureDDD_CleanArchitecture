using CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CleanArchitecture.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class VideoController : ControllerBase
	{
		private readonly IMediator _mediator;

		public VideoController(IMediator mediator)
		{
			_mediator = mediator;
		}

		//! [1] TIPO DE OPERACIÓN, NOMBRE DEL endpoint Y PARÁMETRO QUE SE ENVÍA DESDE EL CLIENTE
		[HttpGet("{username}", Name = "GetVideo")]
		[Authorize]
		//! [1] TIPO DE DATO A DEVOLVER POR ESTE endpoint Y UN STATUS EN CASO DE QUE SEA CORRECTA
		[ProducesResponseType(typeof(IEnumerable<VideosVm>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<IEnumerable<VideosVm>>> GetVideosByUsername(string username)
		{
			//! [1] CREA LA INSTANCIA DEL OBJETO QUERY QUE REQUIERE UN PARÁMETRO
			var query = new GetVideosListQuery(username);
			//! [1] ENVIAMOS EL OBJETO query HACIA APPLICATION
			var video = await _mediator.Send(query);

			return Ok(video);
		}
	}
}
