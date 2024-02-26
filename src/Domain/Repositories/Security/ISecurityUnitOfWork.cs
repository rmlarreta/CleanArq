namespace Domain.Repositories.Security
{
    public interface ISecurityUnitOfWork : IBaseUnitOfWork
    {
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }
        IPermissionRepository Permissions { get; }
        IModuleRepository Modules { get; } 
    }
}

