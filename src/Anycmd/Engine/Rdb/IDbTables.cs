
namespace Anycmd.Engine.Rdb
{
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是关系数据库表集。
    /// </summary>
    public interface IDbTables
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        IReadOnlyDictionary<string, DbTable> this[RdbDescriptor database] { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbTableId"></param>
        /// <param name="dbTable"></param>
        /// <returns></returns>
        bool TryGetDbTable(RdbDescriptor db, string dbTableId, out DbTable dbTable);
    }
}
