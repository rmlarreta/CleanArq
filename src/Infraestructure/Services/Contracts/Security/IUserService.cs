using Domain.Entities.Security;
using Infraestructure.Dtos.Security;

namespace Infraestructure.Services.Contracts.Security
{
    public interface IUserService : IService<UserDto, User, int>
    {
        
    }
}
