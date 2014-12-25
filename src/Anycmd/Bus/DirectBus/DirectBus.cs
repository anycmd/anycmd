
namespace Anycmd.Bus.DirectBus
{
    using Model;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示当总线提交（Commit）时消息将被立即分发的消息总线。
    /// </summary>
    public abstract class DirectBus : DisposableObject, IBus
    {
        #region Private Fields
        private volatile bool _committed = true;
        private readonly IMessageDispatcher _dispatcher;
        private readonly Queue<dynamic> _messageQueue = new Queue<dynamic>();
        private readonly object _queueLock = new object();
        private dynamic[] _backupMessageArray;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>DirectBus</c> 类型的对象。
        /// </summary>
        /// <param name="dispatcher">总线中所使用的消息分发器对象 <see cref="Anycmd.Bus.IMessageDispatcher"/>。</param>
        protected DirectBus(IMessageDispatcher dispatcher)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException("dispatcher");
            }
            this._dispatcher = dispatcher;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 释放当前对象。
        /// </summary>
        /// <param name="disposing">表示当前对象是否应该显式的释放。true表示应显式释放。</param>
        protected override void Dispose(bool disposing)
        {
        }
        #endregion

        #region IBus Members
        /// <summary>
        /// 将给定的消息发布到消息总线。
        /// </summary>
        /// <typeparam name="TMessage">将被发布的消息的类型。</typeparam>
        /// <param name="message">将被发布的消息。</param>
        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            lock (_queueLock)
            {
                _messageQueue.Enqueue(message);
                _committed = false;
            }
        }

        /// <summary>
        /// 将给定的一系列消息发布到消息总线。
        /// </summary>
        /// <typeparam name="TMessage">将被发布的消息的类型。</typeparam>
        /// <param name="messages">将被发布的一个消息系列。</param>
        public void Publish<TMessage>(IEnumerable<TMessage> messages) where TMessage : IMessage
        {
            lock (_queueLock)
            {
                foreach (var message in messages)
                {
                    _messageQueue.Enqueue(message);
                }
                _committed = false;
            }
        }

        /// <summary>
        /// 清空已发布的尚未提交（Commit）的命令。
        /// </summary>
        public void Clear()
        {
            lock (_queueLock)
            {
                this._messageQueue.Clear();
            }
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值，该值表示当前的Unit Of Work是否支持Microsoft分布式事务处理机制。
        /// </summary>
        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        /// <summary>
        /// 获得一个<see cref="System.Boolean"/>值，该值表述了当前的Unit Of Work事务是否已被提交。
        /// </summary>
        public bool Committed
        {
            get { return this._committed; }
        }

        /// <summary>
        /// 提交当前的Unit Of Work事务。
        /// </summary>
        public void Commit()
        {
            lock (_queueLock)
            {
                _backupMessageArray = new dynamic[_messageQueue.Count];
                _messageQueue.CopyTo(_backupMessageArray, 0);
                while (_messageQueue.Count > 0)
                {
                    _dispatcher.DispatchMessage(_messageQueue.Dequeue());
                }
                _committed = true;
            }
        }

        /// <summary>
        /// 回滚当前的Unit Of Work事务。
        /// </summary>
        public void Rollback()
        {
            lock (_queueLock)
            {
                if (_backupMessageArray != null && _backupMessageArray.Length > 0)
                {
                    _messageQueue.Clear();
                    foreach (var msg in _backupMessageArray)
                    {
                        _messageQueue.Enqueue(msg);
                    }
                }
                _committed = false;
            }
        }
        #endregion
    }
}
