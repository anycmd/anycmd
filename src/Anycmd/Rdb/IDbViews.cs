
namespace Anycmd.Rdb
{
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是关系数据库视图集。
    /// </summary>
    public interface IDbViews
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, DbView> this[RdbDescriptor database] { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbViewId"></param>
        /// <param name="dbView"></param>
        /// <returns></returns>
        bool TryGetDbView(RdbDescriptor db, string dbViewId, out DbView dbView);
    }
}
