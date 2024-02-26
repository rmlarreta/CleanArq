using Domain.Entities.Security;
using System.Linq.Expressions;

namespace Domain.Repositories.Security
{
    public interface IModuleRepository : IRepository<Module, int>
    {
        Task<ICollection<Module>> GetAllWithPermissions(Expression<Func<Module, bool>> predicate);
    }
}
