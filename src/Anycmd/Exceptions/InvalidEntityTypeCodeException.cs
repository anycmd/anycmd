
namespace Anycmd.Exceptions
{
    using System;
    using Util;

    public class InvalidEntityTypeCodeException : AnycmdException
    {
        public InvalidEntityTypeCodeException(string message)
            : base(message)
        {
            //Intentionally left blank
        }

        public InvalidEntityTypeCodeException(string message, Exception innerException)
            : base(message, innerException)
        {
            //Intentionally left blank
        }

        public InvalidEntityTypeCodeException(Coder code)
            : base("codespace:" + code.Codespace + ",entityTypeCode:" + code.Code)
        {

        }

        public InvalidEntityTypeCodeException(Coder code, Exception innerException)
            : base("codespace:" + code.Codespace + ",entityTypeCode:" + code.Code, innerException)
        {

        }
    }
}
