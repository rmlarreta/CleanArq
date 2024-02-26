using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Security
{
    public class ModuleDto
    {
        public ModuleDto()
        {
            Permissions = new HashSet<PermissionDto>();
        }

        public int Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        public IEnumerable<PermissionDto> Permissions { get; set; }
    }
}
