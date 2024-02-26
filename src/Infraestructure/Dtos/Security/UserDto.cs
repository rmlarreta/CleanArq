using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Security
{
    public class UserDto
    {
        public int? Id { get; set; }

        [StringLength(1000)]
        public string? Email { get; set; }

        [StringLength(1000)]
        public string? FullName { get; set; }

        public int[]? RolesId { get; set; }

        public int[]? PermissionsId { get; set; }
    }
}
