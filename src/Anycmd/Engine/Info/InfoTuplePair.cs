
namespace Anycmd.Engine.Info
{
    using System;
    using System.Linq;

    /// <summary>
    /// 信息元组夫妻。信息元组夫妻由一个信息标识元组和一个信息值元组组成。
    /// </summary>
    public sealed class InfoTuplePair
    {
        private bool _isSingleGuidGeted = false;
        private bool _isSingleGuid = false;
        private InfoItem _singleGuidItem = null;

        /// <summary>
        /// 构建信息元素。
        /// </summary>
        /// <param name="idItems"></param>
        /// <param name="valueItems"></param>
        public InfoTuplePair(InfoItem[] idItems, InfoItem[] valueItems)
        {
            this.IdTuple = idItems ?? new InfoItem[0];
            this.ValueTuple = valueItems ?? new InfoItem[0];
        }

        /// <summary>
        /// 查看当前信息元组是否基于单列Guid信息标识
        /// </summary>
        public bool IsSingleGuid
        {
            get
            {
                if (!_isSingleGuidGeted)
                {
                    _isSingleGuidGeted = true;
                    _singleGuidItem = this.IdTuple.FirstOrDefault(a => string.Equals("Id", a.Element.Element.Code, StringComparison.OrdinalIgnoreCase));
                    _isSingleGuid = _singleGuidItem != null;
                }
                return _isSingleGuid;
            }
        }

        /// <summary>
        /// 如果当前信息元组基于单列Guid信息标识则返回单列Guid信息标识项，否则返回null
        /// </summary>
        public InfoItem SingleGuidItem
        {
            get
            {
                return IsSingleGuid ? _singleGuidItem : null;
            }
        }

        /// <summary>
        /// 信息标识项。不可能为null
        /// </summary>
        public InfoItem[] IdTuple { get; private set; }

        /// <summary>
        /// 信息值项。不可能为null
        /// </summary>
        public InfoItem[] ValueTuple { get; private set; }
    }
}
