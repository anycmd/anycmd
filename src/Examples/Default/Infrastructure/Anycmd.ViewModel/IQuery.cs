
namespace Anycmd.ViewModel
{
    using Engine.Ac;
    using Model;
    using Query;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是展示层数据查询操作类型。
    /// <remarks>
    /// 操作分两种——命令和查询。该接口的实现类就是属于这个二分法中的查询类别的，该接口的实现类表示的查询是展示层的查询。
    /// </remarks>
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// 查询给定的表或视图和主键标识的数据。
        /// </summary>
        /// <param name="tableOrViewName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        DicReader Get(string tableOrViewName, Guid id);

        /// <summary>
        /// 根据给定的筛选条件分页查询给定的表或视图的数据。
        /// </summary>
        /// <param name="tableOrViewName"></param>
        /// <param name="filterCallback"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlist(string tableOrViewName, Func<SqlFilter> filterCallback, PagingInput paging);

        /// <summary>
        /// 根据给定的筛选条件分页查询给定的实体类型对应的表中的数据。
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="filterCallback"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        List<DicReader> GetPlist(EntityTypeState entityType, Func<SqlFilter> filterCallback, PagingInput paging);
    }
}
