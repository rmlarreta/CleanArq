using AutoMapper;
using Domain.Entities.Security;
using Domain.Repositories.Security;
using Infraestructure.Dtos.Security;
using Infraestructure.Services.Contracts.Security;

namespace Infraestructure.Services.Implementations.Security
{
    public class PermissionService(ISecurityUnitOfWork unitOfWork, IMapper mapper) : Service<PermissionDto, Permission, int, IPermissionRepository, ISecurityUnitOfWork>(unitOfWork, mapper), IPermissionService
    {
        protected override IPermissionRepository GetRepositoryFrom()
        {
            return _unitOfWork.Permissions;
        }
    }
}