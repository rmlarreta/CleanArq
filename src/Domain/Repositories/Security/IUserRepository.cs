using Domain.Entities.Security;
using System.Linq.Expressions;

namespace Domain.Repositories.Security
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<bool> HasPermission(int userId, int permissionId);

        Task<IEnumerable<Permission>?> GetUserPermissions(int userId);

        Task<User?> GetUserWithRolesAndPermissions(Expression<Func<User, bool>> predicate, bool asNoTrack = false);

        Task<User?> GetUserWithRoles(Expression<Func<User, bool>> predicate);

        Task<IEnumerable<Role>?> GetUserRoles(int userId);
    }
}
