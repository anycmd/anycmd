
namespace Anycmd.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 表示Anycmd框架抛出的异常。
    /// </summary>
    [Serializable]
    public class AnycmdException : Exception
    {
        /// <summary>
        /// 初始化一个 <c>BusException</c>类型的对象。
        /// </summary>
        public AnycmdException() : base() { }

        /// <summary>
        /// 使用给定的异常消息字符串初始化一个 <c>BusException</c>类型的对象。
        /// </summary>
        /// <param name="message">描述异常信息的字符串。</param>
        public AnycmdException(string message) : base(message) { }

        /// <summary>
        /// 使用给定的异常消息字符串和引起该异常的内部异常对象初始化一个 <c>BusException</c>类型的对象。
        /// </summary>
        /// <param name="message">描述异常信息的字符串。</param>
        /// <param name="innerException">引发当前异常的内部异常。</param>
        public AnycmdException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// 使用给定的带有格式的异常消息字符串和格式字符串的参数列表初始化一个 <c>BusException</c>类型的对象。
        /// </summary>
        /// <param name="format">带有格式的异常消息字符串。</param>
        /// <param name="args">用于建造异常消息字符串的带有格式的字符串的参数列表。</param>
        public AnycmdException(string format, params object[] args) : base(string.Format(format, args)) { }

        /// <summary>
        /// Initializes a new instance of the System.Exception class with serialized
        ///     data.
        /// </summary>
        /// <param name="info">
        /// The System.Runtime.Serialization.SerializationInfo that holds the serialized
        ///     object data about the exception being thrown.</param>
        /// <param name="context">
        /// The System.Runtime.Serialization.StreamingContext that contains contextual
        ///     information about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException">The info parameter is null.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        /// The class name is null or System.Exception.HResult is zero (0).
        /// </exception>
        protected AnycmdException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        protected Exception Cause { get { return InnerException; } }
    }
}
