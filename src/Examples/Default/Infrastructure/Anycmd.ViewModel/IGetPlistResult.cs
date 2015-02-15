
namespace Anycmd.ViewModel
{
    using Engine.InOuts;
    using Query;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是分页获取实体集时的输入参数类型。
    /// </summary>
    public interface IGetPlistResult : IInputModel
    {
        /// <summary>
        /// 页索引。零基。如果为空视作0。
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 页尺寸。如果为空视作10。
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 排序字段。
        /// </summary>
        string SortField { get; set; }

        /// <summary>
        /// 排序方向。
        /// </summary>
        string SortOrder { get; set; }

        /// <summary>
        /// 设计用于消除一次sql count查询。如果传入的total参数不为0则数据访问层可以避免一次count查询。
        /// 更多信息参见 http://www.cnblogs.com/xuefly/p/3253145.html
        /// </summary>
        long? Total { get; set; }

        /// <summary>
        /// 筛选器列表。// TODO:考虑引入daxnet的UQML，更多信息参见 http://www.cnblogs.com/daxnet/p/3925426.html
        /// </summary>
        List<FilterData> Filters { get; set; }
    }
}
