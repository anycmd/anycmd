
namespace Anycmd
{
    using System;
    using Util;

    public class StringIdAttribute: Attribute, IDAttribute
    {
        public string Value { get; private set; }

        /// <summary>
        /// 不建议在标识上构建层级，比如不建议构建像"Person.User.Add"这样的标识，层级结构体现在别处。
        /// </summary>
        /// <param name="value"></param>
        public StringIdAttribute(string value)
        {
            this.Value = value;
        }
    }
}
