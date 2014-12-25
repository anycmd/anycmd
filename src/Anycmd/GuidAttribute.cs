
namespace Anycmd
{
    using System;
    using Util;

    /// <summary>
    /// 
    /// </summary>
    public class GuidAttribute : Attribute, IDAttribute
    {
        public Guid Value { get; private set; }

        public GuidAttribute(string value)
        {
            this.Value = new Guid(value);
        }
    }
}
