using Domain.Entities.Security;
using System.Linq.Expressions;

namespace Domain.Repositories.Security
{
    public interface IRoleRepository : IRepository<Role, int>
    {
        Task<Role?> GetByName(string name);

        Task<Role?> GetWithPermissions(int roleId);

        Task<Role?> GetWithPermissions(Expression<Func<Role, bool>> predicate);

        Task<Role?> GetWithPermissionsAndUsers(int roleId);

        Task<Role?> GetWithPermissionsModules(Expression<Func<Role, bool>> predicate);

        Task<List<Role>> GetAllWithPermissions(Expression<Func<Role, bool>> predicate);

        Task<List<TProjection>> GetAllWithPermissions<TProjection>(Expression<Func<Role, bool>> predicate, Expression<Func<Role, TProjection>> projection);
    }
}
