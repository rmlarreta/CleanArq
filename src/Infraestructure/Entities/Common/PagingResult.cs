namespace Infraestructure.Entities.Common
{
    public class PagingResult<T> : PagingResultBase
    {
        public IList<T> Results { get; set; }

        public PagingResult()
        {
            Results = new List<T>();
        }
    }
}

