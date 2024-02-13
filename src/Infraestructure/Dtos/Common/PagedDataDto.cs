namespace Infraestructure.Dtos.Common
{
    public class PagedDataDto<T> where T : class
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }

        public IList<T>? Results { get; set; }
    }
}
