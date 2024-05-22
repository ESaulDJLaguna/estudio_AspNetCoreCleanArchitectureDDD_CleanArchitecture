//!=>Referencia [1] [Sección 07, 035. Implementación CQRS y MediatR]

using CleanArchitecture.Domain;

namespace CleanArchitecture.Application.Contracts.Persistence
{
	//! [1] CREAMOS UNA INTERFAZ PERSONALIZADA PARA LOS VÍDEOS
	public interface IVideoRepository : IAsyncRepository<Video>
	{
		Task<Video> GetVideoByNombre(string nombreVideo);
		Task<IEnumerable<Video>> GetVideoByUsername(string username);
	}
}
