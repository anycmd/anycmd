
namespace Anycmd.Engine.Info
{
    using System;

    /// <summary>
    /// 数据项。
    /// <remarks>
    /// 数据项中持有的值是从命令的InfoID或InfoValue字符串中解析的原始值。这些值是只读的，
    /// 因为服务端是不能变更来自client的输入的，如果来自client的输入不合法直接反馈失败信息即可。
    /// </remarks>
    /// </summary>
    public class DataItem
    {
        /// <summary>
        /// 
        /// </summary>
        private DataItem() { }

        /// <summary>
        /// 构造数据项对象
        /// </summary>
        /// <param name="key">数据项键</param>
        /// <param name="value">数据项值</param>
        public DataItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// 读取数据项键。其为命令信息声称的本体元素码。
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 读取数据项值。其为命令信息声称的信息项值。
        /// </summary>
        public string Value { get; private set; }

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
            if (!(obj is DataItem))
            {
                return false;
            }
            var value = obj as DataItem;
            return string.Equals(this.Key, value.Key, StringComparison.OrdinalIgnoreCase)
                && string.Equals(this.Value, value.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (this.Key + this.Value).GetHashCode();
        }
    }
}
