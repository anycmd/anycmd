
namespace Anycmd.Engine.Edi.Abstractions
{
    using Model;
    using System;

    /// <summary>
    /// 本体元素信息项验证器。关联本体元素
    /// </summary>
    public sealed class ElementInfoRule : EntityBase, IAggregateRoot, IElementInfoRule
    {
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 本体元素主键
        /// </summary>
        public Guid ElementId { get; set; }
        /// <summary>
        /// 信息项验证器标识
        /// </summary>
        public Guid InfoRuleId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 有效标记
        /// </summary>
        public int IsEnabled { get; set; }

        public new DateTime CreateOn { get; set; }
    }
}
