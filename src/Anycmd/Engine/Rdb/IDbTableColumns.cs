
namespace Anycmd.Engine.Rdb
{
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是关系数据库表列集。
    /// </summary>
    public interface IDbTableColumns
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <param name="dbTableColumns"></param>
        /// <returns></returns>
        bool TryGetDbTableColumns(RdbDescriptor database, DbTable table, out IReadOnlyDictionary<string, DbTableColumn> dbTableColumns);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableColumnId"></param>
        /// <param name="dbTableColumn"></param>
        /// <returns></returns>
        bool TryGetDbTableColumn(RdbDescriptor database, string tableColumnId, out DbTableColumn dbTableColumn);
    }
}
