using AutoMapper;
using Domain.Entities.Security;
using Domain.Repositories.Security;
using Infraestructure.Dtos.Security;
using Infraestructure.Services.Contracts.Security;

namespace Infraestructure.Services.Implementations.Security
{
    public class UserService(ISecurityUnitOfWork unitOfWork, IMapper mapper) : Service<UserDto, User, int, IUserRepository, ISecurityUnitOfWork>(unitOfWork, mapper), IUserService
    {
        protected override IUserRepository GetRepositoryFrom()
        {
            return _unitOfWork.Users;
        }
    }
}
