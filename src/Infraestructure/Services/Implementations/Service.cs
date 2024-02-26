using AutoMapper;
using Domain.Entities.Common;
using Domain.Repositories;
using Infraestructure.Dtos.Common;
using Infraestructure.Services.Contracts;
using System.Linq.Expressions;

namespace Infraestructure.Services.Implementations
{
    public abstract class Service<TDto, TEntity, TIdentifier, TRepository, TUnitOfWork> : IService<TDto, TEntity, TIdentifier>
         where TDto : class
         where TEntity : Entity<TIdentifier>
         where TRepository : IRepository<TEntity, TIdentifier>
         where TUnitOfWork : IBaseUnitOfWork
    {
        protected readonly TUnitOfWork _unitOfWork;
        protected readonly TRepository _repository;
        protected readonly IMapper _serviceMapper;

        protected Service(TUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = GetRepositoryFrom();
            _serviceMapper = mapper;
        }

        protected int DefaultPageSize => 10;

        protected abstract TRepository GetRepositoryFrom();

        public async Task<TDto> Add(TDto model)
        {
            var entity = await _repository.Add(_serviceMapper.Map<TEntity>(model));
            await _unitOfWork.CommitAsync();
            return _serviceMapper.Map<TDto>(entity);
        }

        public Task Delete(TIdentifier id)
        {
            _repository.Remove(id);
            return Task.CompletedTask;
        }

        public async Task<bool> Exists(TIdentifier identifier)
        {
            return await _repository.Get(identifier) != null;
        }

        public async Task<TDto> Get(Expression<Func<TEntity, bool>> filter)
        {
            var result = await _repository.FirstOrDefault(filter);
            return result == null ? throw new Exception("The requested resource was not found") : _serviceMapper.Map<TDto>(result);

        }

        public async Task<IEnumerable<TDto>> GetAll()
        {
            return _serviceMapper.Map<TDto[]>(await _repository.GetAll());
        }

        public async Task<IEnumerable<TDto>> GetAll(params Expression<Func<TEntity, bool>>[] filters)
        {
            IEnumerable<TEntity> entities;

            if (filters is not null)
                entities = await _repository.GetAll(filters);
            else
                entities = await _repository.GetAll();

            return _serviceMapper.Map<TDto[]>(entities);
        }

        public async Task<IEnumerable<TDto>> GetAllNoTrack()
        {
            return _serviceMapper.Map<TDto[]>(await _repository.GetAllAsNoTrack());
        }

        public async Task<TDto> GetById(TIdentifier id)
        {
            var entity = await _repository.Get(id);
            return entity == null ? throw new Exception("The requested resource was not found") : _serviceMapper.Map<TDto>(entity);

        }

        public async Task<PagedDataDto<TDto>> GetPaged(PagingParameterDto filter)
        {
            var pagedData = await _repository.GetPaged(filter.PageIndex, filter.PageSize, filter.SortExpression, filter.FilterExpression);
            return _serviceMapper.Map<PagedDataDto<TDto>>(pagedData);
        }

        public async Task<PagedDataDto<TDto>> GetPaged(int pageIndex, int pageSize, string sortExpression, string filterExpression)
        {
            var pagedData = await _repository.GetPaged(pageIndex, pageSize, sortExpression, filterExpression);
            return _serviceMapper.Map<PagedDataDto<TDto>>(pagedData);
        }

        public async Task<TProjected> GetProjected<TProjected>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection)
        {
            return await _repository.GetProjected(predicate, projection);
        }

        public Task Update(TDto model)
        {
            _repository.Update(_serviceMapper.Map<TEntity>(model));
            return Task.CompletedTask;
        }
    }
}
