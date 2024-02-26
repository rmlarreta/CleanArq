using Domain.Entities.Security;
using Domain.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories.Security
{
    public class PermissionRepository(DbContext context) : BaseRepository<Permission, int>(context), IPermissionRepository
    {
        public async Task<ICollection<Permission>> GetAllWithModules(Expression<Func<Permission, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Include(q => q.Module)
                              .Where(predicate)
                              .ToListAsync();
        }
    }
}
