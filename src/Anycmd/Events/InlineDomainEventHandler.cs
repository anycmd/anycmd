
namespace Anycmd.Events
{
    using Model;
    using Properties;
    using System;
    using System.Linq;
    using System.Reflection;
    using Util;

    /// <summary>
    /// Represents the domain event handler that is defined within the scope of
    /// an aggregate root and handles the domain event when <c>SourcedAggregateRoot.RaiseEvent&lt;TEvent&gt;</c>
    /// is called.
    /// </summary>
    public sealed class InlineDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {
        #region Private Fields
        private readonly Type _domainEventType;
        private readonly Action<TDomainEvent> _action;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>InlineDomainEventHandler</c> class.
        /// </summary>
        /// <param name="aggregateRoot">The instance of the aggregate root within which the domain event
        /// was raised and handled.</param>
        /// <param name="mi">The method which handles the domain event.</param>
        public InlineDomainEventHandler(ISourcedAggregateRoot aggregateRoot, MethodInfo mi)
        {
            ParameterInfo[] parameters = mi.GetParameters();
            if (parameters == null || !parameters.Any())
            {
                throw new ArgumentException(string.Format(Resources.EX_INVALID_HANDLER_SIGNATURE, mi.Name), "mi");
            }
            _domainEventType = parameters[0].ParameterType;
            _action = domainEvent =>
            {
                try
                {
                    mi.Invoke(aggregateRoot, new object[] { domainEvent });
                }
                catch { }
            };
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Determines whether the specified System.Object is equal to the current System.Object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified System.Object is equal to the current System.Object;
        /// otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == (object)null)
                return false;
            var other = obj as InlineDomainEventHandler<TDomainEvent>;
            if ((object)other == (object)null)
                return false;
            return Object.Equals(this._action, other._action);
        }
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current System.Object.</returns>
        public override int GetHashCode()
        {
            if (this._action != null && this._domainEventType != null)
                return ReflectionHelper.GetHashCode(this._action.GetHashCode(),
                    this._domainEventType.GetHashCode());
            return base.GetHashCode();
        }
        #endregion

        #region IHandler<TDomainEvent> Members
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message to be handled.</param>
        public void Handle(TDomainEvent message)
        {
            _action(message);
        }

        #endregion
    }
}
