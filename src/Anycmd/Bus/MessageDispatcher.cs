
using Anycmd.Commands;

namespace Anycmd.Bus
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 消息分发器。
    /// </summary>
    public class MessageDispatcher : IMessageDispatcher
    {
        #region Private Fields
        /// <summary>
        /// key为消息类型, value为消息处理器列表。
        /// </summary>
        private readonly Dictionary<Type, List<dynamic>> _handlers = new Dictionary<Type, List<dynamic>>();
        #endregion

        #region Protected Methods
        /// <summary>
        /// 当消息分发器将要分发消息前发生。
        /// </summary>
        /// <param name="e">事件数据。</param>
        protected virtual void OnDispatching(MessageDispatchEventArgs e)
        {
            var temp = this.Dispatching;
            if (temp != null)
                temp(this, e);
        }

        /// <summary>
        /// 当完成消息的分发后发生。
        /// </summary>
        /// <param name="e">事件数据。</param>
        protected virtual void OnDispatchFailed(MessageDispatchEventArgs e)
        {
            var temp = this.DispatchFailed;
            if (temp != null)
                temp(this, e);
        }

        /// <summary>
        /// 当完成消息的分发后发生。
        /// </summary>
        /// <param name="e">事件数据。</param>
        protected virtual void OnDispatched(MessageDispatchEventArgs e)
        {
            var temp = this.Dispatched;
            if (temp != null)
                temp(this, e);
        }
        #endregion

        #region IMessageDispatcher Members
        /// <summary>
        /// 清空已注册的消息处理器。
        /// </summary>
        public virtual void Clear()
        {
            _handlers.Clear();
        }

        /// <summary>
        /// 分发给定的消息对象。
        /// </summary>
        /// <param name="message">将被分发的消息对象。</param>
        public virtual void DispatchMessage<T>(T message) where T : IMessage
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }
            var messageType = typeof(T);
            if (!messageType.IsPublic)
            {
                messageType = messageType.BaseType;
            }
            if (messageType != null && !messageType.IsPublic)
            {
                throw new BusException("不支持分发多级继承的消息。消息的继承不应超过两级。");
            }
            if (messageType != null && _handlers.ContainsKey(messageType))
            {
                var messageHandlers = _handlers[messageType];
                foreach (var messageHandler in messageHandlers)
                {
                    var dynMessageHandler = (IHandler<T>)messageHandler;
                    var evtArgs = new MessageDispatchEventArgs(message, messageHandler.GetType(), messageHandler);
                    this.OnDispatching(evtArgs);
                    try
                    {
                        dynMessageHandler.Handle(message);
                        this.OnDispatched(evtArgs);
                    }
                    catch
                    {
                        this.OnDispatchFailed(evtArgs);
                        throw;
                    }
                }
            }
            else
            {
                // TODO:死信邮箱
            }
        }

        /// <summary>
        /// 注册给定的消息处理器到当前消息分发器。
        /// </summary>
        /// <typeparam name="T">消息的.NET类型</typeparam>
        /// <param name="handler">将被注册的消息处理器对象。</param>
        public virtual void Register<T>(IHandler<T> handler) where T : IMessage
        {
            var keyType = typeof(T);

            if (_handlers.ContainsKey(keyType))
            {
                if (typeof(ICommand).IsAssignableFrom(keyType))
                {
                    throw new Exception("一个命令至多对应一个命令处理器");
                }
                var registeredHandlers = _handlers[keyType];
                if (registeredHandlers != null)
                {
                    if (!registeredHandlers.Contains(handler))
                        registeredHandlers.Add(handler);
                }
                else
                {
                    registeredHandlers = new List<dynamic> {handler};
                    _handlers.Add(keyType, registeredHandlers);
                }
            }
            else
            {
                var registeredHandlers = new List<dynamic> {handler};
                _handlers.Add(keyType, registeredHandlers);
            }
        }

        /// <summary>
        /// 从当前消息分发器中反注册给定的消息处理器对象。
        /// </summary>
        /// <typeparam name="T">消息.NET类型。</typeparam>
        /// <param name="handler">将被注册的消息处理器对象。</param>
        public virtual void UnRegister<T>(IHandler<T> handler) where T : IMessage
        {
            var keyType = typeof(T);
            if (_handlers.ContainsKey(keyType) &&
                _handlers[keyType] != null &&
                _handlers[keyType].Count > 0 &&
                _handlers[keyType].Contains(handler))
            {
                _handlers[keyType].Remove(handler);
            }
        }

        /// <summary>
        /// 当消息分发器将要分发消息前发生。
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> Dispatching;

        /// <summary>
        /// 当分发消息失败时发生。
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> DispatchFailed;

        /// <summary>
        /// 当完成消息的分发后发生。
        /// </summary>
        public event EventHandler<MessageDispatchEventArgs> Dispatched;
        #endregion
    }
}
