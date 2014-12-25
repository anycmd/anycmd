
namespace Anycmd.Exceptions
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ValidationException : CoreException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ValidationException(string message) : base(message) { }
    }
}
