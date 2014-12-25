
namespace Anycmd.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是事件聚合器。
    /// </summary>
    /// <remarks>
    /// 更多信息参见: http://msdn.microsoft.com/en-us/library/ff921122(v=pandp.20).aspx
    /// </remarks>
    public interface IEventAggregator
    {
        #region Methods
        /// <summary>
        /// Subscribes the event handler to a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandler">The event handler.</param>
        void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;
        /// <summary>
        /// Subscribes the event handlers to a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlers">The event handlers.</param>
        void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent;
        /// <summary>
        /// Subscribes the event handlers to a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlers">The event handlers.</param>
        void Subscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent;
        /// <summary>
        /// Subscribes the <see cref="Action{T}"/> delegate to a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlerAction">The <see cref="Action{T}"/> delegate.</param>
        void Subscribe<TEvent>(Action<TEvent> eventHandlerAction)
            where TEvent : class, IEvent;
        /// <summary>
        /// Subscribes the <see cref="Action{T}"/> delegates to a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlerActions">The <see cref="Action{T}"/> delegates.</param>
        void Subscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerActions)
            where TEvent : class, IEvent;
        /// <summary>
        /// Subscribes the <see cref="Action{T}"/> delegates to a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlerActions">The <see cref="Action{T}"/> delegates.</param>
        void Subscribe<TEvent>(params Action<TEvent>[] eventHandlerActions)
            where TEvent : class, IEvent;
        /// <summary>
        /// Unsubscribes the event handler from a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandler">The event handler.</param>
        void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;
        /// <summary>
        /// Unsubscribes the event handlers from a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlers">The event handler.</param>
        void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent;
        /// <summary>
        /// Unsubscribes the event handlers from a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlers">The event handlers.</param>
        void Unsubscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent;
        /// <summary>
        /// Unsubscribes the <see cref="Action{T}"/> delegate from a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlerAction">The <see cref="Action{T}"/> delegate.</param>
        void Unsubscribe<TEvent>(Action<TEvent> eventHandlerAction)
            where TEvent : class, IEvent;
        /// <summary>
        /// Unsubscribes the <see cref="Action{T}"/> delegates from a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlerActions">The <see cref="Action{T}"/> delegates.</param>
        void Unsubscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerActions)
            where TEvent : class, IEvent;
        /// <summary>
        /// Unsubscribes the <see cref="Action{T}"/> delegates from a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventHandlerActions">The <see cref="Action{T}"/> delegates.</param>
        void Unsubscribe<TEvent>(params Action<TEvent>[] eventHandlerActions)
            where TEvent : class, IEvent;
        /// <summary>
        /// Unsubscribes all the subscribed event handlers from a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        void UnsubscribeAll<TEvent>()
            where TEvent : class, IEvent;
        /// <summary>
        /// Unsubscribes all the event handlers from the event aggregator.
        /// </summary>
        void UnsubscribeAll();
        /// <summary>
        /// Gets the subscribed event handlers for a given event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <returns>A collection of subscribed event handlers.</returns>
        IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>()
            where TEvent : class, IEvent;
        /// <summary>
        /// Publishes the event to all of its subscriptions.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to be published.</typeparam>
        /// <param name="event">The event to be published.</param>
        void Publish<TEvent>(TEvent @event)
            where TEvent : class, IEvent;
        /// <summary>
        /// Publishes the event to all of its subscriptions.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event to be published.</typeparam>
        /// <param name="event">The event to be published.</param>
        /// <param name="callback">The callback method to be executed after the event has been published and processed.</param>
        /// <param name="timeout">When the event handler is executing in parallel, represents the timeout value
        /// for the handler to complete.</param>
        void Publish<TEvent>(TEvent @event, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent;
        #endregion
    }
}
