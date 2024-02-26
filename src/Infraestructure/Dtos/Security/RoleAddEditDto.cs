using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Security
{
    public class RoleAddEditDto
    {
        public RoleAddEditDto()
        {
            PermissionIds = [];
        }

        public int? Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }
        
        [MaxLength(180)]
        public string? Description { get; set; }
        
        public List<int> PermissionIds { get; set; }

    }
}
