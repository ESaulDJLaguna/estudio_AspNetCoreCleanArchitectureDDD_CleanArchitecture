//!=>Referencia [1] [Sección 07, 036. Query Command y Handlers]

using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using MediatR;

namespace CleanArchitecture.Application.Features.Videos.Queries.GetVideosList
{
	//![1] ES LA CLASE QUE IMPLEMENTA LA FUNCIONALIDAD DEL QUERY. LOS PARÁMETROS DE LA INTERFAZ IMPELENTADA SON: EL ORIGEN DEL MENSAJE (GetVideosListQuery) Y EL RESUTLADO DESEADO
	public class GetVideosListQueryHandler : IRequestHandler<GetVideosListQuery, List<VideosVm>>
	{
		//![1] INTERFAZ DONDE SE DEFINIÓ LA OPERACIÓN A UTILIZAR
		//private readonly IVideoRepository _videoRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public GetVideosListQueryHandler(IVideoRepository videoRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			//_videoRepository = videoRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		//![1] MÉTODO DE IRequestHandler DONDE SE IMPLEMENTA LA LÓGICA PARA HACER LA CONSULTA A LA BASE DE DATOS.
		//! GetVideosListQuery REPRESENTA LOS PARÁMETROS DISPONIBLES
		public async Task<List<VideosVm>> Handle(GetVideosListQuery request, CancellationToken cancellationToken)
		{
			//var videoList = await _videoRepository.GetVideoByUsername(request._Username);
			var videoList = await _unitOfWork.VideoRepository.GetVideoByUsername(request._Username);

			return _mapper.Map<List<VideosVm>>(videoList);
		}
	}
}
