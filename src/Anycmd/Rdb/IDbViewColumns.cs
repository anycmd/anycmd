
namespace Anycmd.Rdb
{
    using System.Collections.Generic;
    
    /// <summary>
    /// 表示该接口的实现类是关系数据库视图列集。
    /// </summary>
    public interface IDbViewColumns
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="view"></param>
        /// <param name="dbViewColumns"></param>
        /// <returns></returns>
        bool TryGetDbViewColumns(RdbDescriptor database, DbView view, out IReadOnlyDictionary<string, DbViewColumn> dbViewColumns);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="viewColumnId"></param>
        /// <param name="dbViewColumn"></param>
        /// <returns></returns>
        bool TryGetDbViewColumn(RdbDescriptor database, string viewColumnId, out DbViewColumn dbViewColumn);
    }
}
