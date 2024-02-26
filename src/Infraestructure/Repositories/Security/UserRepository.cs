using Domain.Entities.Security;
using Domain.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories.Security
{
    public class UserRepository(DbContext context) : BaseRepository<User, int>(context), IUserRepository
    {
        protected override IQueryable<User> LoadRelations(IQueryable<User> query)
        {
            return base.LoadRelations(query);
        }

        public async Task<IEnumerable<Permission>?> GetUserPermissions(int userId)
        {
            return await _dbSet.Include(q => q.UserRoles)
                               .ThenInclude(q => q.Role)
                               .ThenInclude(q => q.RolePermissions)
                               .ThenInclude(q => q.Permission)
                               .Where(q => q.Id == userId && q.DeletedDate == null)
                               .SelectMany(q => q.UserRoles)
                               .Select(q => q.Role)
                               .SelectMany(q => q.RolePermissions)
                               .Select(q => q.Permission)
                               .Distinct()
                               .ToListAsync();
        }

        public async Task<User?> GetUserWithRolesAndPermissions(Expression<Func<User, bool>> predicate, bool asNoTrack = false)
        {
            IQueryable<User> user = _dbSet;
            if (asNoTrack)
                user = _dbSet.AsNoTracking();
            return await user.Include(q => q.UserRoles)
                               .ThenInclude(q => q.Role)
                               .ThenInclude(q => q.RolePermissions)
                               .ThenInclude(q => q.Permission)
                               .Where(q => q.DeletedDate == null)
                               .FirstOrDefaultAsync(predicate);
        }

        public async Task<User?> GetUserWithRoles(Expression<Func<User, bool>> predicate)
        {
            return await _dbSet.Include(q => q.UserRoles)
                               .ThenInclude(q => q.Role)
                               .Where(q => q.DeletedDate == null)
                               .Where(predicate)
                               .FirstOrDefaultAsync();
        }

        public async Task<bool> HasPermission(int userId, int permissionId)
        {
            return await _dbSet.Include(q => q.UserRoles)
                               .ThenInclude(q => q.Role)
                               .ThenInclude(q => q.RolePermissions)
                               .ThenInclude(q => q.Permission)
                               .AnyAsync(u => u.Id == userId &&
                                              u.DeletedDate == null &&
                                              u.UserRoles.Any(p => p.Role.RolePermissions.Any(r => r.PermissionId == permissionId))
                                        );
        }

        public async Task<IEnumerable<Role>?> GetUserRoles(int userId)
        {

            return await _dbSet.Include(q => q.UserRoles)
                               .ThenInclude(q => q.Role)
                               .Where(q => q.Id == userId && q.DeletedDate == null)
                               .SelectMany(q => q.UserRoles)
                               .Select(q => q.Role)
                               .ToListAsync();
        }
    }
}
