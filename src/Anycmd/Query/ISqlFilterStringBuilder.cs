
namespace Anycmd.Query
{
    using Engine.Rdb;
    using System.Collections.Generic;
    using System.Data.Common;

    /// <summary>
    /// 将数据查询筛选器转化为查询条件字符串的接口
    /// </summary>
    public interface ISqlFilterStringBuilder
    {
        /// <summary>
        /// 将给定的数据查询筛选器列表转化为原生sql查询条件字符串返回
        /// </summary>
        /// <param name="rdb"></param>
        /// <param name="filters">数据查询筛选器</param>
        /// <param name="alias">别名，空字符串或null表示无别名</param>
        /// <param name="prams"></param>
        /// <returns></returns>
        string FilterString(RdbDescriptor rdb, List<FilterData> filters, string alias, out List<DbParameter> prams);
    }
}
