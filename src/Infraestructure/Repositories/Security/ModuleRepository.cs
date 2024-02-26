using Domain.Entities.Security;
using Domain.Repositories.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repositories.Security
{
    public class ModuleRepository(DbContext context) : BaseRepository<Module, int>(context), IModuleRepository
    {
        public async Task<ICollection<Module>> GetAllWithPermissions(Expression<Func<Module, bool>> predicate)
        {
            return await _dbSet.AsNoTracking()
                .Include(q => q.Permissions)
                .Where(predicate)
                .ToListAsync();
        }
    }
}
