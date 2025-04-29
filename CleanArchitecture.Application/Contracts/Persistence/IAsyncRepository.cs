//!=>Referencia [1] [Sección 05, 030. Func vs Expressions]
//!=>Referencia [2 [Sección 05. Creación de interface genérica]

using CleanArchitecture.Domain.Common;
using System.Linq.Expressions;

namespace CleanArchitecture.Application.Contracts.Persistence
{
	public interface IAsyncRepository<T> where T : BaseDomainModel
	{
		//![1] DEVUELVE LOS REGISTROS DE UNA ENTIDAD DETERMINADA
		Task<IReadOnlyList<T>> GetAllAsync();

		//![1] DEVUELVE UNA COLECCIÓN DE DATOS DE ACUERDO A UNA CONDICIÓN LÓGICA QUE SE PASA COMO PARÁMETRO.
		Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

		//![2] PERMITIRÁ INCLUIR EN LOS PARÁMETROS EL ORDENAMIENTO CON EL QUE SE DEVOLVERÁ EL RESULTADO
		Task<IReadOnlyList<T>> GetAsync(
			Expression<Func<T, bool>> predicated = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeString = null,
			bool disableTracking = true);

		//![2] SE UTILIZARÁ PARA IMPLEMENTAR UNA PAGINACIÓN
		Task<IReadOnlyList<T>> GetAsync(
			Expression<Func<T, bool>> predicated = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			List<Expression<Func<T, object>>> includes = null,
			bool disableTracking = true);

		//![2] DEVOLVERÁ EL OBJETO QUE PERTENECE AL id INDICADO
		Task<T> GetByIdAsync(int id);

		//![2] AGREGA UN NUEVO REGISTRO DE CUALQUIER ENTIDAD
		Task<T> AddAsync(T entity);

		//![2] ACTUALIZA UN REGISTRO
		Task<T> UpdateAsync(T entity);

		//![2] ELIMINA UN REGISTRO ESPECÍFICO
		Task DeleteAsync(T entity);
	}
}
