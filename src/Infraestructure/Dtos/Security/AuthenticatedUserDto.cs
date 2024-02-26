using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Security
{
    public class AuthenticatedUserDto
    {
        public int UserId { get; set; }
       
        [MaxLength(100)]
        public string? FullUserName { get; set; }
        
        [MaxLength(100)]
        public string? UserName { get; set; }
        
        public PermissionDto[]? Permissions { get; set; }
         
        public RoleDto[]? Roles { get; internal set; }
        
        public int RefreshTime { get; internal set; }
    }
}
