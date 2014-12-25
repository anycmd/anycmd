
namespace Anycmd.Query
{
    using Exceptions;
    using System;

    /// <summary>
    /// 分页输入参数
    /// </summary>
    public class PagingInput
    {
        private long? _total;

        public PagingInput() { }

        /// <summary>
        /// 构造分页参数
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortOrder">排序方向</param>
        public PagingInput(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.SortField = sortField;
            this.SortOrder = sortOrder;
        }

        /// <summary>
        /// 构造分页参数
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortOrder">排序方向</param>
        /// <param name="total">总记录数</param>
        public PagingInput(int pageIndex, int pageSize, string sortField, string sortOrder, int? total)
            : this(pageIndex, pageSize, sortField, sortOrder)
        {
            this.Total = total;
        }

        /// <summary>
        /// 页索引。零基索引，即第一页对应0
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页尺寸
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序方向
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// 归档数据有一个特性，就是它的总记录数是不改变的，所以传入total以渐少一次数据库count查询
        /// </summary>
        public long? Total
        {
            get
            {
                return _total.HasValue ? (int)_total.Value : 0;
            }
            set
            {
                if (value.HasValue)
                {
                    _total = value.Value;
                }
            }
        }

        /// <summary>
        /// 查看total字段是否为空或者0
        /// </summary>
        private bool IsTotalNullOrZero { get { return !Total.HasValue || Total.Value == 0; } }

        /// <summary>
        /// pageSize * pageIndex的计算值
        /// </summary>
        public int SkipCount { get { return PageSize * PageIndex; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        public void Count(Func<int> func)
        {
            if (this.IsTotalNullOrZero && func != null)
            {
                this.Total = func();
            }
        }

        /// <summary>
        /// 判断分页输入参数是否合法
        /// </summary>
        /// <returns></returns>
        public void Valid()
        {
            if (string.IsNullOrEmpty(SortField) || string.IsNullOrEmpty(SortOrder))
            {
                throw new ValidationException("排序是必须的");
            }
            if (SortOrder.ToLower() != "asc" && SortOrder.ToLower() != "desc")
            {
                throw new ValidationException("排序方向只能是asc或desc");
            }
        }
    }
}
