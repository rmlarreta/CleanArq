using Domain.Entities.Common;
using Domain.Entities.Security;
using Infraestructure.Dtos.Common;
using Infraestructure.Dtos.Security;

namespace Infraestructure.Services.Contracts.Security
{
    public interface IRoleService : IService<RoleDto, Role, int>
    {
        Task<PagedDataDto<RoleDto>> GetPaged(RoleFilterDto filter);
        Task<RoleAddEditDto> Add(RoleAddEditDto dto);
        Task<int> Update(RoleAddEditDto dto);
    }
}
