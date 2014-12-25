
namespace Anycmd.Events
{
    using System;
    using Util;

    /// <summary>
    /// 表示将事件处理委托给一个 <see cref="Action{T}"/> 委托方法的事件处理器。
    /// </summary>
    /// <typeparam name="TEvent">将被处理的事件.NET类型。</typeparam>
    public sealed class ActionDelegatedEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        #region Private Fields
        private readonly Action<TEvent> _action;
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>ActionDelegatedEventHandler{TEvent}</c> 类型对象.
        /// </summary>
        /// <param name="action">事件处理过程被委托给该 <see cref="Action{T}"/> 委托对象。</param>
        public ActionDelegatedEventHandler(Action<TEvent> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            this._action = action;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a <see cref="Boolean"/> value which indicates whether the current
        /// <c>ActionDelegatedEventHandler{T}</c> equals to the given object.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> which is used to compare to
        /// the current <c>ActionDelegatedEventHandler{T}</c> instance.</param>
        /// <returns>If the given object equals to the current <c>ActionDelegatedEventHandler{T}</c>
        /// instance, returns true, otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            var other = obj as ActionDelegatedEventHandler<TEvent>;
            return other != null && Object.Equals(this._action, other._action);
        }

        /// <summary>
        /// Gets the hash code of the current <c>ActionDelegatedEventHandler{T}</c> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return ReflectionHelper.GetHashCode(this._action.GetHashCode());
        }
        #endregion

        #region IHandler<TDomainEvent> Members
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(TEvent message)
        {
            _action(message);
        }

        #endregion
    }
}
