using Domain.Repositories.Security;
using Infraestructure.Data;

namespace Infraestructure.Repositories.Security
{
    public class SecurityUnitOfWork(DefaultContext context) : BaseUnitOfWork(context), ISecurityUnitOfWork
    {
        private IRoleRepository? _roles;
        private IUserRepository? _users;
        private IPermissionRepository? _permissions;
        private IModuleRepository? _modules;

        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IPermissionRepository Permissions => _permissions ??= new PermissionRepository(_context);
        public IModuleRepository Modules => _modules ??= new ModuleRepository(_context);
    }
}
