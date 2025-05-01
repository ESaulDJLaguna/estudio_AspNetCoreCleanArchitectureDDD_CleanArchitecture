using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Application.Contracts.Persistence
{
	public interface IUnitOfWork : IDisposable
	{
		public IStreamerRepository StreamerRepository { get; }
		public IVideoRepository VideoRepository { get; }


		// Creamos un método genérico que devuelva la instancia del servicio repositorio que quiero utilizar
		IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel;

		// Método para saber cuando una transacción ya ha culminado
		Task<int> Complete();
	}
}
