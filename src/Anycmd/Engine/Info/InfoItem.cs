
namespace Anycmd.Engine.Info
{
    using Engine.Edi;
    using System;

    /// <summary>
    /// 信息项描述对象。
    /// <remarks>
    /// 信息项描述对象根据本体元素描述对象和数据项<see cref="DataItem"/>值构建，
    /// 所以只有正确信息键的数据项<see cref="DataItem"/>才会构建信息项描述对象。
    /// </remarks>
    /// </summary>
    public sealed class InfoItem : DataItem
    {
        /// <summary>
        /// 构建信息项描述对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">信息项值</param>
        private InfoItem(string key, string value)
            : base(key, value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static InfoItem Create(ElementDescriptor element, string value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return new InfoItem(element.Element.Code, value)
            {
                Element = element
            };
        }

        /// <summary>
        /// 本体元素描述对象。
        /// <remarks>不可能为null。只有正确信息键的数据项才会构建信息项描述对象。</remarks>
        /// </summary>
        public ElementDescriptor Element
        {
            get;
            private set;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var value = obj as InfoItem;
            if (value == null)
            {
                return false;
            }
            return this.Element == value.Element
                && string.Equals(this.Key, value.Key, StringComparison.OrdinalIgnoreCase)
                && string.Equals(this.Value, value.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.Element.GetHashCode() + (this.Key + this.Value).GetHashCode();
        }
    }
}
