
namespace Anycmd.Bus.AggregatorBus
{
    using Events;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 表示使用聚合器实现的消息总线。
    /// </summary>
    public abstract class AggregatorBus : DisposableObject, IBus
    {
        #region Private Fields
        private readonly Queue<IMessage> _messageQueue = new Queue<IMessage>();
        private readonly IEventAggregator _eventAggregator;
        private readonly MethodInfo _publishMethod;
        private readonly object _sync = new object();
        private bool _committed = true;
        private IMessage[] _backupMessageArray;
        #endregion

        #region Ctor
        /// <summary>
        /// 实例化一个 <c>AggregatorBus</c> 类型的对象。
        /// </summary>
        protected AggregatorBus(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }
            this._eventAggregator = eventAggregator;
            _publishMethod = (from m in this._eventAggregator.GetType().GetMethods()
                             let parameters = m.GetParameters()
                             let methodName = m.Name
                             where methodName == "Publish" &&
                             parameters != null &&
                             parameters.Length == 1
                             select m).First();
        }
        #endregion

        #region Private Methods
        private void PublishInternal<TMessage>(TMessage message) where TMessage : IMessage
        {
            _messageQueue.Enqueue(message);
            _committed = false;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 释放当前对象。
        /// </summary>
        /// <param name="disposing">表示当前对象是否应该显式的释放。true表示应显式释放。</param>
        protected override void Dispose(bool disposing) { }
        #endregion

        #region IBus Members
        /// <summary>
        /// 将给定的消息发布到消息总线。
        /// </summary>
        /// <typeparam name="TMessage">将被发布的消息的类型。</typeparam>
        /// <param name="message">将被发布的消息。</param>
        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            lock (_sync)
            {
                PublishInternal<TMessage>(message);
            }
        }

        /// <summary>
        /// 将给定的一系列消息发布到消息总线。
        /// </summary>
        /// <typeparam name="TMessage">将被发布的消息的类型。</typeparam>
        /// <param name="messages">将被发布的一个消息系列。</param>
        public void Publish<TMessage>(IEnumerable<TMessage> messages) where TMessage : IMessage
        {
            lock (_sync)
            {
                foreach (var message in messages)
                    PublishInternal<TMessage>(message);
            }
        }

        /// <summary>
        /// 清空已发布的尚未提交（Commit）的命令。
        /// </summary>
        public void Clear()
        {
            lock (_sync)
            {
                _messageQueue.Clear();
                _committed = true;
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
            get { return _committed; }
        }

        /// <summary>
        /// 提交当前的Unit Of Work事务。
        /// </summary>
        public void Commit()
        {
            lock (_sync)
            {
                _backupMessageArray = new IMessage[_messageQueue.Count];
                _messageQueue.CopyTo(_backupMessageArray, 0);
                while (_messageQueue.Count > 0)
                {
                    var @event = _messageQueue.Dequeue();
                    var @eventType = @event.GetType();
                    var method = _publishMethod.MakeGenericMethod(@eventType);
                    method.Invoke(this._eventAggregator, new object[] { @event });
                }
                _committed = true;
            }
        }

        /// <summary>
        /// 回滚当前的Unit Of Work事务。
        /// </summary>
        public void Rollback()
        {
            lock (_sync)
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
