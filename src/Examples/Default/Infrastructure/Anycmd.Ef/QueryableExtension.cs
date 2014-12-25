
namespace Anycmd.Ef
{
    using Query;
    using System.Linq;

    public static class QueryableExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="query"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public static IQueryable<TSource> SetPaging<TSource>(this IQueryable<TSource> query, PagingInput paging)
        {
            return query.Skip(paging.SkipCount).Take(paging.PageSize);
        }
    }
}
