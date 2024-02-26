using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Security
{
    public class UserDetailDto
    {

        [StringLength(1000)]
        public string? Name { get; set; }


        [StringLength(1000)]
        public string? Email { get; set; }

        [StringLength(1000)]
        public string? Role { get; set; }


    }
}
