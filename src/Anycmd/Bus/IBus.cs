
namespace Anycmd.Bus
{
    using Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示消息总线。
    /// </summary>
    public interface IBus : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// 将给定的消息发布到消息总线。
        /// </summary>
        /// <typeparam name="TMessage">将被发布的消息的类型。</typeparam>
        /// <param name="message">将被发布的消息。</param>
        void Publish<TMessage>(TMessage message) where TMessage : IMessage;

        /// <summary>
        /// 将给定的一系列消息发布到消息总线。
        /// </summary>
        /// <typeparam name="TMessage">将被发布的消息的类型。</typeparam>
        /// <param name="messages">将被发布的一个消息系列。</param>
        void Publish<TMessage>(IEnumerable<TMessage> messages) where TMessage : IMessage;

        /// <summary>
        /// 清空已发布的尚未提交（Commit）的命令。
        /// </summary>
        void Clear();
    }
}
