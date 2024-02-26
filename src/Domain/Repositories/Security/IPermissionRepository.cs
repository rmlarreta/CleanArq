using Domain.Entities.Security;
using System.Linq.Expressions;

namespace Domain.Repositories.Security
{
    public interface IPermissionRepository : IRepository<Permission, int>
    {
        Task<ICollection<Permission>> GetAllWithModules(Expression<Func<Permission, bool>> predicate);
    }
}
