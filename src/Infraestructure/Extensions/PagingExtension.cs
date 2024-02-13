using Infraestructure.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class PagingExtension
    {
        public static async Task<PagingResult<T>> GetPaged<T>(this IQueryable<T> query,
            int page, int pageSize) where T : class
        {
            var result = new PagingResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = await query.CountAsync()
            };


            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }

        public static async Task<PagingResult<T>> GetPaged<T>(this IQueryable<T> query, int page, int pageSize, string sortExpression, string filterExpression) where T : class
        {
            var result = new PagingResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize
            };

            if (!string.IsNullOrEmpty(filterExpression))
                query = query.Where(filterExpression);

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            result.RowCount = await query.CountAsync();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);


            var skip = page * pageSize;
            query = query.Skip(skip).Take(pageSize);
            result.Results = await query.ToListAsync();
            return result;
        }

        public static async Task<PagingResult<TProjected>> GetPaged<T, TProjected>(this IQueryable<T> query, int page, int pageSize, string sortExpression, string filterExpression, Expression<Func<T, TProjected>> projection)
        {
            var result = new PagingResult<TProjected>
            {
                CurrentPage = page,
                PageSize = pageSize
            };

            if (!string.IsNullOrEmpty(filterExpression))
                query = query.Where(filterExpression);

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            result.RowCount = await query.CountAsync();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = page * pageSize;
            query = query.Skip(skip).Take(pageSize);
            result.Results = await query.Select(projection).ToListAsync();
            return result;
        }

        public static async Task<PagingResult<TProjected>> GetPaged<T, TProjected>(this IQueryable<T> query, int page, int pageSize, string sortExpression, Expression<Func<T, bool>> predicate, Expression<Func<T, TProjected>> projection)
        {
            var result = new PagingResult<TProjected>
            {
                CurrentPage = page,
                PageSize = pageSize
            };

            if (predicate != null)
                query = query.Where(predicate);

            if (!string.IsNullOrEmpty(sortExpression))
                query = query.OrderBy(sortExpression);

            result.RowCount = await query.CountAsync();

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = page * pageSize;
            query = query.Skip(skip).Take(pageSize);
            result.Results = await query.Select(projection).ToListAsync();
            return result;
        }
    }
}
