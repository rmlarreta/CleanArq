using Infraestructure.Dtos.Common;
using Infraestructure.Entities.Common;
using System.Linq.Expressions;

namespace Infraestructure.Interfaces
{
    public interface IService<TDto, TEntity, TIdentifier> where TDto : class where TEntity : Entity<TIdentifier>
    {

        Task<TDto> GetById(TIdentifier id);

        Task<TDto> Get(Expression<Func<TEntity, bool>> filter);

        Task<TProjected> GetProjected<TProjected>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjected>> projection);

        Task<IEnumerable<TDto>> GetAll();

        Task<IEnumerable<TDto>> GetAllNoTrack();

        Task<IEnumerable<TDto>> GetAll(params Expression<Func<TEntity, bool>>[] filters);

        Task<PagedDataDto<TDto>> GetPaged(PagingParameterDto filter);

        Task<PagedDataDto<TDto>> GetPaged(int pageIndex, int pageSize, string sortExpression, string filterExpression);

        Task<TDto> Add(TDto model);

        Task Update(TDto model);

        Task Delete(TIdentifier id);

        Task<bool> Exists(TIdentifier identifier);

    }
}
