using System.ComponentModel.DataAnnotations;

namespace Infraestructure.Dtos.Common
{
    public class PagingParameterDto
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        [MaxLength(int.MaxValue)]
        public string SortExpression { get; set; } = null!;

        [MaxLength(int.MaxValue)]
        public string FilterExpression { get; set; } = null!;
    }
}
