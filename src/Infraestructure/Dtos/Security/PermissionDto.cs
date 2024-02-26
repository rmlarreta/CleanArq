using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Security
{
    public class PermissionDto
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; }

        public int ModuleId { get; set; }
    }
}
