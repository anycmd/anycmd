
namespace Anycmd
{
    using System;
    using Util;

    public class Int32IdAttribute : Attribute, IDAttribute
    {
        public Int32 Value { get; private set; }

        public Int32IdAttribute(Int32 value)
        {
            this.Value = value;
        }
    }
}
