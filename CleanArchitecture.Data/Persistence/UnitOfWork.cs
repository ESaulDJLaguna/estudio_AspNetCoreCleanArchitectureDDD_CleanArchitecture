using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infrastructure.Repositories;
using System.Collections;

namespace CleanArchitecture.Infrastructure.Persistence
{
	// UnitOfWork como instancia, va a almacenar la colección de referencias a los servicios repositorios
	public class UnitOfWork : IUnitOfWork
	{
		private Hashtable _repositories;
		// También se necesita una referencia (una instancia) a Entity Framework Core (el dbContext)
		private readonly StreamerDbContext _context;

		private IVideoRepository _videoRepository;
		private IStreamerRepository _streamerRepository;


        // Las inyecciones NO se harán desde el constructor, sino vía propiedades
        public IVideoRepository VideoRepository => _videoRepository ?? new VideoRepository(_context);
        public IStreamerRepository StreamerRepository => _streamerRepository ?? new StreamerRepository(_context);
		

        public UnitOfWork(StreamerDbContext context)
		{
			_context = context;
		}

		// Este método es que se encargará de disparar la confirmación de todas las transacciones que se están realizando.
		// De ahora en adelante, el encargado de confirmar las transacciones, operaciones, va a ser UnitOfWork, ya no será cada referencia de repositorio
		public async Task<int> Complete()
		{
			return await _context.SaveChangesAsync();
		}

		// Quiero que se elimine el context cuando la transacción haya culminado
		public void Dispose()
		{
			_context.Dispose();
		}

		// Instancia del repositorio
		public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel
		{
			if(_repositories is null)
			{
				_repositories = new Hashtable();
			}

			// Se capturará el nombre de la entidad que se está trabajando
			var type = typeof(TEntity).Name;

			if(!_repositories.ContainsKey(type))
			{
				var repositoryType = typeof(RepositoryBase<>);
				var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
				_repositories.Add(type, repositoryInstance);
			}

			return (IAsyncRepository<TEntity>)_repositories[type];
		}
	}
}
