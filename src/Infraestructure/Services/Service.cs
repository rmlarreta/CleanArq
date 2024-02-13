using AutoMapper;
using Infraestructure.Dtos.Common;
using Infraestructure.Entities.Common;
using Infraestructure.Interfaces;
using System.Linq.Expressions;

namespace Infraestructure.Services
{
    public abstract class Service<TDto, TEntity, TIdentifier, TRepository, TUnitOfWork> : IService<TDto, TEntity, TIdentifier>
          where TDto : class
          where TEntity : Entity<TIdentifier>
          where TRepository : IRepository<TEntity, TIdentifier>
          where TUnitOfWork : IBaseUnitOfWork
    {
        protected readonly TUnitOfWork unitOfWork;
        protected readonly TRepository repository;
        protected readonly IMapper serviceMapper;

        protected Service(TUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            repository = GetRepositoryFrom();
            serviceMapper = mapper;
        }

        protected int DefaultPageSize => 10;

        protected abstract TRepository GetRepositoryFrom();

        public virtual async Task<TDto> Add(TDto dto)
        {
            var entity = await repository.Add(serviceMapper.Map<TEntity>(dto));
            await unitOfWork.CommitAsync();
            return serviceMapper.Map<TDto>(entity);
        }

        public virtual Task Delete(TIdentifier id)
        {
            repository.Remove(id);
            return Task.CompletedTask;
        }

        public virtual async Task<bool> Exists(TIdentifier identifier)
        {
            return await repository.Get(identifier) != null;
        }

        public virtual async Task<TDto> GetById(TIdentifier id)
        {
            var entity = await repository.Get(id);
            return entity == null ? throw new Exception("The requested resource was not found") : serviceMapper.Map<TDto>(entity);
        }

        public virtual async Task<TDto> Get(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await repository.FirstOrDefault(predicate);
            return result == null ? throw new Exception("The requested resource was not found") : serviceMapper.Map<TDto>(result);
        }

        public async Task<TProjected> GetProjected<TProjected>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection)
        {
            return await repository.GetProjected(predicate, projection);
        }


        public virtual async Task<IEnumerable<TDto>> GetAll()
        {
            return serviceMapper.Map<TDto[]>(await repository.GetAll());
        }

        public virtual async Task<IEnumerable<TDto>> GetAllNoTrack()
        {
            return serviceMapper.Map<TDto[]>(await repository.GetAllAsNoTrack());
        }

        public async Task<IEnumerable<TDto>> GetAll(params Expression<Func<TEntity, bool>>[] predicates)
        {
            IEnumerable<TEntity> entities;

            if (predicates != null)
                entities = await repository.GetAll(predicates);
            else
                entities = await repository.GetAll();

            return serviceMapper.Map<TDto[]>(entities);
        }

        public virtual async Task<PagedDataDto<TDto>> GetPaged(PagingParameterDto filter)
        {
            var pagedData = await repository.GetPaged(filter.PageIndex, filter.PageSize, filter.SortExpression, filter.FilterExpression);
            return serviceMapper.Map<PagedDataDto<TDto>>(pagedData);
        }
        public virtual async Task<PagedDataDto<TDto>> GetPaged(int pageIndex, int pageSize, string sortExpression, string filterExpression)
        {
            var pagedData = await repository.GetPaged(pageIndex, pageSize, sortExpression, filterExpression);
            return serviceMapper.Map<PagedDataDto<TDto>>(pagedData);
        }

        public virtual Task Update(TDto dto)
        {
            repository.Update(serviceMapper.Map<TEntity>(dto));
            return Task.CompletedTask;
        }
    }
}
