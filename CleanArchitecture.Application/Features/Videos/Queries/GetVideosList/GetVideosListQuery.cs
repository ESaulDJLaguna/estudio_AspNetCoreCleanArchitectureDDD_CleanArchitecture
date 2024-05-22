//!=>Referencia [1] [Sección 07, 036. Query Command y Handlers]

using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList
{
	//![1] IRequest PERMITE LA COMUNICACIÓN ENTRE EL Query Y EL QueryHandler. REQUIERE QUE SE LE INDIQUE EL TIPO DE DATO A DEVOLVER: <List<VideosVm>>.
	//! BÁSICAMENTE GetVideosListQuery SON LOS PARÁMETROS NECESARIOS POR EL GetVideosListQueryHandler
	public class GetVideosListQuery : IRequest<List<VideosVm>>
	{
		public string _Username { get; set; } = String.Empty;

        public GetVideosListQuery(string username)
        {
			_Username = username ?? throw new ArgumentNullException(nameof(username));
        }
    }
}
