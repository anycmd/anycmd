
namespace Anycmd.Bus
{
    using System;

    /// <summary>
    /// 表示分发消息时的事件数据。
    /// </summary>
    public class MessageDispatchEventArgs : EventArgs
    {
        #region Public Properties
        /// <summary>
        /// 获取或设置消息对象。
        /// </summary>
        public dynamic Message { get; set; }

        /// <summary>
        /// 获取或设置消息处理器类型。
        /// </summary>
        public Type HandlerType { get; set; }

        /// <summary>
        /// 获取或设置消息处理器。
        /// </summary>
        public object Handler { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>MessageDispatchEventArgs</c> 类型的对象。
        /// </summary>
        public MessageDispatchEventArgs() { }

        /// <summary>
        /// 初始化一个 <c>MessageDispatchEventArgs</c> 类型的对象。
        /// </summary>
        /// <param name="message">消息对象。</param>
        /// <param name="handlerType">消息处理器类型。</param>
        /// <param name="handler">消息处理器。</param>
        public MessageDispatchEventArgs(dynamic message, Type handlerType, object handler)
        {
            this.Message = message;
            this.HandlerType = handlerType;
            this.Handler = handler;
        }
        #endregion
    }
}
