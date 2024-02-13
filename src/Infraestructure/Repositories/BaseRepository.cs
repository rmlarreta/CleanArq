using Infraestructure.Entities.Common;
using Infraestructure.Interfaces;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Infraestructure.Repositories
{
    public abstract class BaseRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : Entity<TIdentifier>
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected virtual IQueryable<TEntity> LoadRelations(IQueryable<TEntity> query)
        {
            return query;
        }

        protected BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> Add(TEntity entity)
        {
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, bool>>[] predicates)
        {
            return await ApplyFilters(LoadRelations(_dbSet), predicates).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(IEnumerable<string> includes, params Expression<Func<TEntity, bool>>[] predicates)
        {
            var set = _dbSet;
            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    set.Include(include);
                }
            }
            var query = LoadRelations(set);
            return await ApplyFilters(query, predicates).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllTop(IEnumerable<string> includes, Expression<Func<TEntity, bool>> predicate, int top)
        {
            var query = LoadRelations(_dbSet);
            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query.Include(include);
                }
            }
            return await ApplyFilter(query, predicate).Take(top).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllTop(Expression<Func<TEntity, bool>> predicate, int top)
        {
            var query = LoadRelations(_dbSet);
            return await ApplyFilter(query, predicate).Take(top).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await LoadRelations(_dbSet).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsNoTrack()
        {
            return await LoadRelations(_dbSet).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsNoTrack(params Expression<Func<TEntity, bool>>[] predicates)
        {
            return await ApplyFilters(LoadRelations(_dbSet), predicates).AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity> Get(TIdentifier id)
        {
            var entity = await _dbSet.FindAsync(id) ?? throw new Exception($"Entity with ID {id} not found.");
            await LoadReferences(entity);
            return entity;
        }

        public virtual async Task<PagingResult<TEntity>> GetPaged(int pageIndex, int pageSize, string sortExpression, string filterExpression)
        {
            return await _dbSet.GetPaged(pageIndex, pageSize, sortExpression, filterExpression);
        }

        public virtual async Task<PagingResult<TProjected>> GetPaged<TProjected>(int pageIndex, int pageSize, string sortExpression, string filterExpression, Expression<Func<TEntity, TProjected>> projection)
        {
            return await _dbSet.GetPaged(pageIndex, pageSize, sortExpression, filterExpression, projection);
        }

        public virtual async Task<PagingResult<TProjected>> GetPaged<TProjected>(int pageIndex, int pageSize, string sortExpression, Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection)
        {
            return await _dbSet.GetPaged(pageIndex, pageSize, sortExpression, predicate, projection);
        }

        public async Task<TProjected> GetProjected<TProjected>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection, bool noTracking = false)
        {
            var query = _dbSet.AsQueryable();
            if (noTracking)
                query = _dbSet.AsNoTracking();

            var result = await ApplyFilter(query, predicate).Select(projection).FirstOrDefaultAsync();
            return result == null ? throw new Exception($"Entity not found.") : result;
        }

        public async Task<IEnumerable<TProjected>> GetProjectedMany<TProjected>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection, bool noTracking = false)
        {
            var query = _dbSet.AsQueryable();
            if (noTracking)
                query = _dbSet.AsNoTracking();

            return await ApplyFilter(query, predicate).Select(projection).ToListAsync();
        }
        public async Task<IEnumerable<TProjected>> GetProjectedOrderedMany<TProjected, TOrdered>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection, Expression<Func<TEntity, TOrdered>> order, bool noTracking = false, bool orderAscending = true)
        {
            var query = _dbSet.AsQueryable();
            if (noTracking)
                query = _dbSet.AsNoTracking();

            if (orderAscending)
            {
                return await ApplyFilter(query, predicate).OrderBy(order).Select(projection).ToListAsync();
            }

            return await ApplyFilter(query, predicate).OrderByDescending(order).Select(projection).ToListAsync();
        }

        public async Task<IEnumerable<TProjected>> GetProjectedOrderedManyByTop<TProjected, TOrdered>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection, Expression<Func<TEntity, TOrdered>> order, int topRowSelected, bool noTracking = false, bool orderAscending = true)
        {
            var query = _dbSet.AsQueryable();
            if (noTracking)
                query = _dbSet.AsNoTracking();

            if (orderAscending)
            {
                return await ApplyFilter(query, predicate).OrderBy(order).Select(projection).ToListAsync();
            }

            return await ApplyFilter(query, predicate).OrderByDescending(order).Take(topRowSelected).Select(projection).ToListAsync();
        }

        protected virtual Task<TEntity> LoadReferences(TEntity entity)
        {
            return Task.FromResult(entity);
        }

        protected IQueryable<T> ApplyFilters<T>(IQueryable<T> source, Expression<Func<T, bool>>[] filters) where T : Entity<TIdentifier>
        {
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    source = source.Where(filter);
                }
            }

            if (typeof(T).BaseType == typeof(Auditable))
            {
                source = source.Where("x => !x.DeletedDate.HasValue");
            }


            return source;
        }

        protected IQueryable<T> ApplyFilter<T>(IQueryable<T> source, Expression<Func<T, bool>> filter) where T : Entity<TIdentifier>
        {
            if (filter != null)
            {
                source = source.Where(filter);
            }

            if (typeof(T).BaseType == typeof(Auditable))
            {
                source = source.Where("x => !x.DeletedDate.HasValue");
            }
            return source;
        }

        protected IQueryable<T> ApplyFilter<T>(IQueryable<T> source) where T : Entity<TIdentifier>
        {
            if (typeof(T).BaseType == typeof(Auditable))
            {
                source = source.Where("x => !x.DeletedDate.HasValue");
            }
            return source;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }
        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Remove(TIdentifier id)
        {
            var existing = _context.Find<TEntity>(id);
            if (existing != null) _dbSet.Remove(existing);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await ApplyFilter(LoadRelations(_dbSet), predicate).FirstOrDefaultAsync();
            return result ?? throw new Exception($"Entity not found.");
        }

        public virtual async Task<int> Count()
        {
            return await ApplyFilter(_dbSet).CountAsync();
        }

        public virtual async Task<int> Count(Expression<Func<TEntity, bool>> predicate)
        {
            return await ApplyFilter(_dbSet, predicate).CountAsync();
        }

        public virtual async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return await ApplyFilter(_dbSet, predicate).AnyAsync();
        }
    }
}
