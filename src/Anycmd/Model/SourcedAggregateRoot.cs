
namespace Anycmd.Model
{
    using Events;
    using Snapshots;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using Util;

    /// <summary>
    /// Represents the base class for aggregate roots that support the event sourcing mechanism.
    /// </summary>
    public abstract class SourcedAggregateRoot : ISourcedAggregateRoot
    {
        #region Private Fields
        private Guid _id;
        private long _version;
        private long _eventVersion;
        private long _branch;
        private readonly List<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();
        private readonly Dictionary<Type, List<object>> _domainEventHandlers = new Dictionary<Type, List<object>>();
        #endregion

        #region Internal Constants
        /// <summary>
        /// The name of the method that updates the aggregate root version
        /// and clears the uncommitted events.
        /// </summary>
        internal const string UpdateVersionAndClearUncommittedEventsMethodName = @"UpdateVersionAndClearUncommittedEvents";
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>SourcedAggregateRoot</c> class.
        /// </summary>
        protected SourcedAggregateRoot()
            : this(Guid.NewGuid())
        {

        }
        /// <summary>
        /// Initializes a new instance of <c>SourcedAggregateRoot</c> class.
        /// </summary>
        /// <param name="id">The unique identifier of the aggregate root.</param>
        protected SourcedAggregateRoot(Guid id)
        {
            this._id = id;
            this._version = Constants.ApplicationRuntime.DefaultVersion;
            this._eventVersion = this._version;
            this._branch = Constants.ApplicationRuntime.DefaultBranch;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets all the valid domain event handlers for the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event on which the handlers should be retrieved.</param>
        /// <returns>The domain event handlers.</returns>
        private IEnumerable<object> GetDomainEventHandlers(IDomainEvent domainEvent)
        {
            Type eventType = domainEvent.GetType();
            if (_domainEventHandlers.ContainsKey(eventType))
                return _domainEventHandlers[eventType];
            else
            {
                var handlers = new List<object>();
                // firstly create and add all the handler methods defined within the aggregation root.
                MethodInfo[] allMethods = this.GetType().GetMethods(BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.Instance);
                var handlerMethods = from method in allMethods
                                     let returnType = method.ReturnType
                                     let @params = method.GetParameters()
                                     let handlerAttributes = method.GetCustomAttributes(typeof(HandlesAttribute), false)
                                     where returnType == typeof(void) &&
                                     @params != null &&
                                     @params.Any() &&
                                     @params[0].ParameterType == eventType &&
                                     handlerAttributes != null &&
                                     ((HandlesAttribute)handlerAttributes[0]).DomainEventType == eventType
                                     select new { MethodInfo = method };
                foreach (var handlerMethod in handlerMethods)
                {
                    var inlineDomainEventHandlerType = typeof(InlineDomainEventHandler<>).MakeGenericType(eventType);
                    var inlineDomainEventHandler = Activator.CreateInstance(inlineDomainEventHandlerType,
                        new object[] { this, handlerMethod.MethodInfo });
                    handlers.Add(inlineDomainEventHandler);
                }
                // then read all the registered handlers.

                _domainEventHandlers.Add(eventType, handlers);
                return handlers;
            }
        }
        /// <summary>
        /// Handles the given domain event on the aggregate root.
        /// </summary>
        /// <typeparam name="TEvent">The type of the domain event.</typeparam>
        /// <param name="event">The domain event which needs to be handled by aggregate root.</param>
        private void HandleEvent<TEvent>(TEvent @event)
            where TEvent : IDomainEvent
        {
            var handlers = this.GetDomainEventHandlers(@event);
            foreach (var handler in handlers)
            {
                var handleMethod = handler.GetType().GetMethod("Handle", BindingFlags.Public | BindingFlags.Instance);
                if (handleMethod != null)
                {
                    handleMethod.Invoke(handler, new object[] { @event });
                }
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Raises a domain event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the domain event.</typeparam>
        /// <param name="event">The domain event to be raised.</param>
        protected virtual void RaiseEvent<TEvent>(TEvent @event) where TEvent : DomainEvent
        {
            @event.Id = Guid.NewGuid();
            @event.Version = ++_eventVersion;
            @event.Source = this;
            @event.AssemblyQualifiedEventType = typeof(TEvent).AssemblyQualifiedName;
            @event.Branch = Constants.ApplicationRuntime.DefaultBranch;
            @event.Timestamp = DateTime.UtcNow;
            this.HandleEvent<TEvent>(@event);
            _uncommittedEvents.Add(@event);
        }
        /// <summary>
        /// When overrided in the derived classes, builds the aggregate from the given snapshot instance.
        /// </summary>
        /// <param name="snapshot">The <see cref="Anycmd.Snapshots.ISnapshot"/> instance
        /// with which the aggregate is built.</param>
        protected abstract void DoBuildFromSnapshot(ISnapshot snapshot);
        /// <summary>
        /// When overrided in the derived classes, creates the snapshot instance based on the current aggregate.
        /// </summary>
        /// <returns>The <see cref="Anycmd.Snapshots.ISnapshot"/> instance that is created based on
        /// the current aggregate.</returns>
        protected abstract ISnapshot DoCreateSnapshot();
        /// <summary>
        /// Updates the version of the aggregate root and clears all uncommitted events.
        /// </summary>
        /// <remarks>This method is handled by the anycmd framework internally and should not be referenced in any circumstances.</remarks>
        [Obsolete(@"This method is handled by the anycmd framework internally 
and should not be referenced in any circumstances.", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void UpdateVersionAndClearUncommittedEvents()
        {
            this._version = Version;
            this._uncommittedEvents.Clear();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the hash code for current aggregate root.
        /// </summary>
        /// <returns>The calculated hash code for the current aggregate root.</returns>
        public override int GetHashCode()
        {
            return ReflectionHelper.GetHashCode(this.Id.GetHashCode(),
                this.UncommittedEvents.GetHashCode(),
                this.Version.GetHashCode(),
                this.Branch.GetHashCode());
        }
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value indicating whether this instance is equal to a specified
        /// object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>True if obj is an instance of the <see cref="ISourcedAggregateRoot"/> type and equals the value of this
        /// instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            var other = obj as SourcedAggregateRoot;
            if ((object)other == (object)null)
                return false;
            return other.Id == this.Id;
        }
        #endregion

        #region ISourcedAggregateRoot Members
        /// <summary>
        /// Builds the aggreate from the historial events.
        /// </summary>
        /// <param name="historicalEvents">The historical events from which the aggregate is built.</param>
        public virtual void BuildFromHistory(IEnumerable<IDomainEvent> historicalEvents)
        {
            if (this._uncommittedEvents.Any())
                this._uncommittedEvents.Clear();
            var domainEvents = historicalEvents as IDomainEvent[] ?? historicalEvents.ToArray();
            foreach (var de in domainEvents)
                this.HandleEvent<IDomainEvent>(de);
            this._version = domainEvents[domainEvents.Length - 1].Version;
            this._eventVersion = this._version;
        }
        /// <summary>
        /// Gets all the uncommitted events.
        /// </summary>
        public virtual IEnumerable<IDomainEvent> UncommittedEvents
        {
            get { return _uncommittedEvents; }
        }
        /// <summary>
        /// Gets the version of the aggregate.
        /// </summary>
        public virtual long Version
        {
            get { return this._version + _uncommittedEvents.Count; }
        }
        /// <summary>
        /// Gets the branch on which the aggregate exists.
        /// </summary>
        public virtual long Branch
        {
            get { return this._branch; }
        }
        #endregion

        #region IEntity Members
        /// <summary>
        /// Gets or sets the identifier of the aggregate root.
        /// </summary>
        public virtual Guid Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        #endregion

        #region IOrignator Members
        /// <summary>
        /// Builds the aggregate from the specified snapshot instance.
        /// </summary>
        /// <param name="snapshot">The <see cref="Anycmd.Snapshots.ISnapshot"/> instance
        /// with which the aggregate is built.</param>
        public virtual void BuildFromSnapshot(ISnapshot snapshot)
        {
            this._branch = snapshot.Branch;
            this._version = snapshot.Version;
            this._id = snapshot.AggregateRootId;
            DoBuildFromSnapshot(snapshot);
            this._uncommittedEvents.Clear();
        }
        /// <summary>
        /// Creates the snapshot instance based on the current aggregate.
        /// </summary>
        /// <returns>The <see cref="Anycmd.Snapshots.ISnapshot"/> instance that is created based on
        /// the current aggregate.</returns>
        public virtual ISnapshot CreateSnapshot()
        {
            var snapshot = this.DoCreateSnapshot();
            snapshot.Branch = this.Branch;
            snapshot.Version = this.Version;
            snapshot.Timestamp = DateTime.UtcNow;
            snapshot.AggregateRootId = this._id;
            return snapshot;
        }
        #endregion
    }
}
