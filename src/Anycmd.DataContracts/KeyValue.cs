
namespace Anycmd.DataContracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 数据项。
    /// <remarks>数据项中持有的值是从命令的InfoId或InfoValue字符串中解析的原始值</remarks>
    /// </summary>
    [DataContract]
    public sealed class KeyValue : IDto
    {
        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        public KeyValue() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public KeyValue(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public KeyValue(KeyValuePair<string, string> item)
        {
            this.Key = item.Key;
            this.Value = item.Value;
        }
        #endregion

        /// <summary>
        /// 数据项键。其为命令信息声称的本体元素码。
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// 数据项值。其为命令信息声称的信息项值。
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}
