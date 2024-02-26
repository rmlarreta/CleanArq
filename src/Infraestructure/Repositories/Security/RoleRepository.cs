using Domain.Entities.Security;
using Domain.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories.Security
{
    public class RoleRepository(DbContext context) : BaseRepository<Role, int>(context), IRoleRepository
    {
        public async Task<List<Role>> GetAllWithPermissions(Expression<Func<Role, bool>> predicate)
        {
            return await _dbSet.Include(q => q.RolePermissions)
                              .Where(predicate)
                              .ToListAsync();
        }

        public async Task<List<TProjection>> GetAllWithPermissions<TProjection>(Expression<Func<Role, bool>> predicate, Expression<Func<Role, TProjection>> projection)
        {
            return await _dbSet.Include(q => q.RolePermissions)
                               .Where(predicate)
                               .Select(projection)
                               .ToListAsync();
        }

        public async Task<Role?> GetByName(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(q => q.Name.Equals(name));
        }

        public async Task<Role?> GetWithPermissions(int roleId)
        {
            return await _dbSet.Include(q => q.RolePermissions)
                              .FirstOrDefaultAsync(q => q.Id == roleId);
        }

        public async Task<Role?> GetWithPermissionsAndUsers(int roleId)
        {
            return await _dbSet
                .Include(q => q.RolePermissions)
                .Include(q => q.UserRoles)
                .FirstOrDefaultAsync(q => q.Id == roleId);
        }

        public async Task<Role?> GetWithPermissions(Expression<Func<Role, bool>> predicate)
        {
            return await _dbSet.Include(q => q.RolePermissions)
                              .FirstOrDefaultAsync(predicate);
        }

        public async Task<Role?> GetWithPermissionsModules(Expression<Func<Role, bool>> predicate)
        {
            return await _dbSet.Include(q => q.RolePermissions)
                              .ThenInclude(q => q.Permission)
                              .ThenInclude(q => q.Module)
                              .Where(predicate)
                              .FirstOrDefaultAsync();
        }
    }
}
