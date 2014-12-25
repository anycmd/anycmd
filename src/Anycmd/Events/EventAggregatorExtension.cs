
namespace Anycmd.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class EventAggregatorExtension
    {
        /// <summary>
        /// Publishes the domain event to the registered domain event handlers.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event to be published.</typeparam>
        /// <param name="eag"></param>
        /// <param name="domainEvent">The domain event to be published.</param>
        /// <remarks>
        /// This method publishes domain events to the domain event handlers that have been registered 
        /// to the object container. The method will use the <see>
        ///         <cref>ServiceProvider</cref>
        ///     </see>
        ///     instance to
        /// resolve all the registered domain event handlers, then publish the given domain event to
        /// all of these registered handlers. The domain event handler should implement the interface
        /// <see cref="IDomainEventHandler{T}"/>.
        /// </remarks>
        public static void Publish<TDomainEvent>(this IEventAggregator eag, TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            var handlers = eag.GetSubscriptions<TDomainEvent>();
            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    if (handler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
                    {
                        var handler1 = handler;
                        Task.Factory.StartNew(() => handler1.Handle(domainEvent));
                    }
                    else
                        handler.Handle(domainEvent);
                }
            }
        }

        /// <summary>
        /// Publishes the domain event to the registered domain event handlers.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event to be published.</typeparam>
        /// <param name="eag"></param>
        /// <param name="domainEvent">The domain event to be published.</param>
        /// <param name="callback">The callback function which will be executed after the
        /// domain event has been published and processed.</param>
        /// <param name="timeout">If a domain event handler is decorated by <see cref="ParallelExecutionAttribute"/> attribute, this parameter
        /// is to specify the timeout value for the handler to process the event.</param>
        /// <remarks>
        /// This method publishes domain events to the domain event handlers that have been registered 
        /// to the object container. The method will use the <see>
        ///         <cref>ServiceProvider</cref>
        ///     </see>
        ///     instance to
        /// resolve all the registered domain event handlers, then publish the given domain event to
        /// all of these registered handlers. The domain event handler should implement the interface
        /// <see cref="IDomainEventHandler{T}"/>.
        /// </remarks>
        public static void Publish<TDomainEvent>(this IEventAggregator eag, TDomainEvent domainEvent, Action<TDomainEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TDomainEvent : class, IDomainEvent
        {
            var handlers = eag.GetSubscriptions<TDomainEvent>();
            if (handlers != null && handlers.Any())
            {
                var tasks = new List<Task>();
                try
                {
                    foreach (var handler in handlers)
                    {
                        if (handler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
                        {
                            var handler1 = handler;
                            tasks.Add(Task.Factory.StartNew(() => handler1.Handle(domainEvent)));
                        }
                        else
                            handler.Handle(domainEvent);
                    }
                    if (tasks.Count > 0)
                    {
                        if (timeout == null)
                            Task.WaitAll(tasks.ToArray());
                        else
                            Task.WaitAll(tasks.ToArray(), timeout.Value);
                    }
                    callback(domainEvent, true, null);
                }
                catch (Exception ex)
                {
                    callback(domainEvent, false, ex);
                }
            }
            else
                callback(domainEvent, false, null);
        }
    }
}
