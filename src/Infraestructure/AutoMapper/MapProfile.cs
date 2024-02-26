using AutoMapper;
using Domain.Entities.Security;
using Infraestructure.Dtos.Security;

namespace Infraestructure.AutoMapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Permission, PermissionDto>().ReverseMap();
          
            CreateMap<Role, RoleDto>()
                .ForMember(q => q.PermissionIds, opt => opt.MapFrom(src => src.RolePermissions != null ? src.RolePermissions.Select(r => r.PermissionId).ToList() : new List<int>()));
           
            
            CreateMap<User, UserDto>()
                .ForMember(q => q.RolesId, opt => opt.MapFrom(src => src.UserRoles != null ? src.UserRoles.Select(r => r.RoleId).ToList() : new List<int>()))                 
                .IgnoreMember(i => i.PermissionsId)
                .ReverseMap();

        }
    }
}
