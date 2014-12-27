
namespace Anycmd.Bus
{
    using System;

    /// <summary>
    /// 表示消息分发器。
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// 清空已注册的消息处理器。
        /// </summary>
        void Clear();

        /// <summary>
        /// 分发给定的消息对象。
        /// </summary>
        /// <param name="message">将被分发的消息对象。</param>
        void DispatchMessage<TMessage>(TMessage message) where TMessage : IMessage;

        /// <summary>
        /// 注册给定的消息处理器到当前消息分发器。
        /// </summary>
        /// <typeparam name="TMessage">消息的.NET类型</typeparam>
        /// <param name="handler">将被注册的消息处理器对象。</param>
        void Register<TMessage>(IHandler<TMessage> handler) where TMessage : IMessage;

        /// <summary>
        /// 从当前消息分发器中反注册给定的消息处理器对象。
        /// </summary>
        /// <typeparam name="TMessage">消息.NET类型。</typeparam>
        /// <param name="handler">将被注册的消息处理器对象。</param>
        void UnRegister<TMessage>(IHandler<TMessage> handler) where TMessage : IMessage;

        /// <summary>
        /// 当消息分发器将要分发消息前发生。
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> Dispatching;

        /// <summary>
        /// 当分发消息失败时发生。
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> DispatchFailed;

        /// <summary>
        /// 当完成消息的分发后发生。
        /// </summary>
        event EventHandler<MessageDispatchEventArgs> Dispatched;
    }
}
