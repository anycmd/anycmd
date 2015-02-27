
namespace Anycmd.Ef
{
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// 
    /// </summary>
    public static class EfDbContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="queryString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ObjectQuery<TEntity> CreateQuery<TEntity>(
            this RdbContext dbContext, string queryString, params ObjectParameter[] parameters)
            where TEntity : class
        {
            dbContext.Set<TEntity>();
            return ((IObjectContextAdapter)dbContext).ObjectContext.CreateQuery<TEntity>(queryString, parameters);
        }
    }
}
