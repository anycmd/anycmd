
namespace Anycmd.Exceptions
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class NotExistException : CoreException
    {
        #region Ctor

        public NotExistException() : base() { }

        public NotExistException(string message) : base(message) { }

        public NotExistException(string message, Exception innerException) : base(message, innerException) { }

        public NotExistException(string format, params object[] args) : base(string.Format(format, args)) { }
        #endregion
    }
}
