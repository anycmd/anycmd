
namespace Anycmd.ViewModel
{
    using Query;
    using System.Collections.Generic;

    /// <summary>
    /// 表示分页获取实体集时的输入参数类型。
    /// </summary>
    public class GetPlistResult : PagingInput, IGetPlistResult
    {
        private List<FilterData> _filters;

        /// <summary>
        /// 筛选器列表。// TODO:考虑引入daxnet的UQML，更多信息参见 http://www.cnblogs.com/daxnet/p/3925426.html
        /// </summary>
        public List<FilterData> Filters
        {
            get { return _filters ?? (_filters = new List<FilterData>()); }
            set
            {
                _filters = value;
            }
        }
    }
}
