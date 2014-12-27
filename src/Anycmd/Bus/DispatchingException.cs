
namespace Anycmd.Bus
{
    using Exceptions;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 表示分发消息的时候发生的异常。
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(_Exception))]
    public class DispatchingException : AnycmdException
    {
        #region Ctor
        /// <summary>
        /// 初始化一个<c>DispatcherException</c>类型的对象。
        /// </summary>
        public DispatchingException() : base() { }

        /// <summary>
        /// 使用给定的异常消息字符串初始化一个 <c>DispatchingException</c>类型的对象。
        /// </summary>
        /// <param name="message">描述异常信息的字符串。</param>
        public DispatchingException(string message) : base(message) { }

        /// <summary>
        /// 使用给定的异常消息字符串和引起该异常的内部异常对象初始化一个 <c>DispatchingException</c>类型的对象。
        /// </summary>
        /// <param name="message">描述异常信息的字符串。</param>
        /// <param name="innerException">引发当前异常的内部异常。</param>
        public DispatchingException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// 使用给定的带有格式的异常消息字符串和格式字符串的参数列表初始化一个 <c>DispatchingException</c>类型的对象。
        /// </summary>
        /// <param name="format">带有格式的异常消息字符串。</param>
        /// <param name="args">用于建造异常消息字符串的带有格式的字符串的参数列表。</param>
        public DispatchingException(string format, params object[] args) : base(string.Format(format, args)) { }
        #endregion
    }
}
