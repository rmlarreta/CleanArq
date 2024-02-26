using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Security
{
    public class RoleDto
    {
        public RoleDto()
        {
            PermissionIds = [];
        }

        public int Id { get; set; }
 
        public string? Name { get; set; }
        
        [MaxLength(180)]
        public string? Description { get; set; }

        public List<int> PermissionIds { get; set; }
    }
}
