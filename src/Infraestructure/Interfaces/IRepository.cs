using Infraestructure.Entities.Common; 
using System.Linq.Expressions;

namespace Infraestructure.Interfaces
{
    public interface IRepository<TEntity, TIdentifier> where TEntity : Entity<TIdentifier>
    {
        Task<TEntity> Get(TIdentifier id);

        Task<PagingResult<TEntity>> GetPaged(int pageIndex, int pageSize, string sortExpression, string filterExpression);

        Task<PagingResult<TProjected>> GetPaged<TProjected>(int pageIndex, int pageSize, string sortExpression, string filterExpression, Expression<Func<TEntity, TProjected>> projection);

        Task<PagingResult<TProjected>> GetPaged<TProjected>(int pageIndex, int pageSize, string sortExpression, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection);

        Task<TProjected> GetProjected<TProjected>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection, bool noTracking = false);

        Task<IEnumerable<TProjected>> GetProjectedMany<TProjected>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection, bool noTracking = false);

        Task<IEnumerable<TProjected>> GetProjectedOrderedMany<TProjected, TOrdered>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection, Expression<Func<TEntity, TOrdered>> order, bool noTracking = false, bool orderAscending = true);

        Task<IEnumerable<TProjected>> GetProjectedOrderedManyByTop<TProjected, TOrdered>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection, Expression<Func<TEntity, TOrdered>> order, int topRowSelected, bool noTracking = false, bool orderAscending = true);

        Task<IEnumerable<TEntity>> GetAll();

        Task<IEnumerable<TEntity>> GetAllAsNoTrack();

        Task<IEnumerable<TEntity>> GetAllAsNoTrack(params Expression<Func<TEntity, bool>>[] predicates);

        Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, bool>>[] predicates);

        Task<IEnumerable<TEntity>> GetAll(IEnumerable<string> includes, params Expression<Func<TEntity, bool>>[] predicates);

        Task<IEnumerable<TEntity>> GetAllTop(IEnumerable<string> includes, Expression<Func<TEntity, bool>> predicate, int top);

        Task<IEnumerable<TEntity>> GetAllTop(Expression<Func<TEntity, bool>> predicate, int top);

        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Add(TEntity entity);

        Task AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        void Remove(TIdentifier id);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task<int> Count();

        Task<int> Count(Expression<Func<TEntity, bool>> predicate);

        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);

        void UpdateRange(IEnumerable<TEntity> entities);
    }

}
