
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
