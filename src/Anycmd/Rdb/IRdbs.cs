
namespace Anycmd.Rdb
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是关系数据库集。
    /// </summary>
    public interface IRdbs : IEnumerable<RdbDescriptor>
    {
        /// <summary>
        /// 关系数据库表列数据集
        /// </summary>
        IDbTableColumns DbTableColumns { get; }
        /// <summary>
        /// 关系数据库表数据集
        /// </summary>
        IDbTables DbTables { get; }
        /// <summary>
        /// 关系数据库视图列数据集
        /// </summary>
        IDbViewColumns DbViewColumns { get; }
        /// <summary>
        /// 关系数据库视图数据集
        /// </summary>
        IDbViews DbViews { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        bool TryDb(Guid dbId, out RdbDescriptor db);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbId"></param>
        /// <returns></returns>
        bool ContainsDb(Guid dbId);
    }
}
