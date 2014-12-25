using System;
using System.Runtime.InteropServices;

namespace Anycmd.Serialization
{
    using Exceptions;

    /// <summary>
    /// Represents errors that occur when serializing/deserializing an object.
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class SerializationException : CoreException
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of the <c>SerializationException</c> class.
        /// </summary>
        public SerializationException() : base() { }
        /// <summary>
        /// Initializes a new instance of the <c>SerializationException</c> class with the specified
        /// error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SerializationException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <c>SerializationException</c> class with the specified
        /// error message and the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The inner exception that is the cause of this exception.</param>
        public SerializationException(string message, Exception innerException) : base(message, innerException) { }
        /// <summary>
        /// Initializes a new instance of the <c>SerializationException</c> class with the specified
        /// string formatter and the arguments that are used for formatting the message which
        /// describes the error.
        /// </summary>
        /// <param name="format">The string formatter which is used for formatting the error message.</param>
        /// <param name="args">The arguments that are used by the formatter to build the error message.</param>
        public SerializationException(string format, params object[] args) : base(string.Format(format, args)) { }
        #endregion
    }
}
