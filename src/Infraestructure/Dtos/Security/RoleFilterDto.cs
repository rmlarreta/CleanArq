using Infraestructure.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Security
{
    public class RoleFilterDto : PagingParameterDto
    {
        [StringLength(1000)]
        public string? Name { get; set; }
    }
}
