
namespace Anycmd.Repositories
{
    using Bus;
    using Events;
    using Events.Storage;
    using Snapshots.Providers;
    using System;
    using System.Linq;
    using System.Transactions;

    /// <summary>
    /// 表示支持事件溯源的领域仓储。
    /// </summary>
    public class EventSourcedDomainRepository : EventPublisherDomainRepository
    {
        #region Private Fields
        private readonly IDomainEventStorage _domainEventStorage;
        private readonly ISnapshotProvider _snapshotProvider;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>EventSourcedDomainRepository</c> class.
        /// </summary>
        /// <param name="domainEventStorage">The <see cref="Anycmd.Events.Storage.IDomainEventStorage"/> instance
        /// that handles the storage mechanism for domain events.</param>
        /// <param name="eventBus">The <see cref="Anycmd.Bus.IEventBus"/> instance to which the domain events
        /// are published.</param>
        /// <param name="snapshotProvider">The <see cref="Anycmd.Snapshots.Providers.ISnapshotProvider"/> instance
        /// that is used for handling the snapshot operations.</param>
        public EventSourcedDomainRepository(IDomainEventStorage domainEventStorage, IEventBus eventBus, ISnapshotProvider snapshotProvider)
            : base(eventBus)
        {
            this._domainEventStorage = domainEventStorage;
            this._snapshotProvider = snapshotProvider;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Commits the changes registered in the domain repository.
        /// </summary>
        protected override void DoCommit()
        {
            // firstly we save and publish the event via domain event storage
            // and the event bus.
            foreach (var aggregateRoot in this.SaveHash)
            {
                if (this._snapshotProvider != null && this._snapshotProvider.Option == SnapshotProviderOption.Immediate)
                {
                    if (this._snapshotProvider.CanCreateOrUpdateSnapshot(aggregateRoot))
                    {
                        this._snapshotProvider.CreateOrUpdateSnapshot(aggregateRoot);
                    }
                }
                var events = aggregateRoot.UncommittedEvents;
                foreach (var evt in events)
                {
                    _domainEventStorage.SaveEvent(evt);
                    this.EventBus.Publish(evt);
                }
            }
            // then commit the save/publish via UoW.
            if (this.DistributedTransactionSupported)
            {
                // the distributed transaction is supported either by domain event storage
                // or by the event bus. use the MS-DTC (Distributed Transaction Coordinator)
                // to commit the transaction. This solves the 2PC for deivces that are
                // distributed transaction compatible.
                using (var ts = new TransactionScope())
                {
                    _domainEventStorage.Commit();
                    this.EventBus.Commit();
                    if (this._snapshotProvider != null && this._snapshotProvider.Option == SnapshotProviderOption.Immediate)
                    {
                        this._snapshotProvider.Commit();
                    }
                    ts.Complete();
                }
            }
            else
            {
                _domainEventStorage.Commit();
                this.EventBus.Commit();
                if (this._snapshotProvider != null && this._snapshotProvider.Option == SnapshotProviderOption.Immediate)
                {
                    this._snapshotProvider.Commit();
                }
            }
        }
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.Committed)
                {
                    try
                    {
                        this.Commit();
                    }
                    catch
                    {
                        this.Rollback();
                        throw;
                    }
                }
                _domainEventStorage.Dispose();
                if (_snapshotProvider != null)
                    _snapshotProvider.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the instance of the domain event storage which is used for storing domain events.
        /// </summary>
        public IDomainEventStorage DomainEventStorage
        {
            get { return this._domainEventStorage; }
        }
        /// <summary>
        /// Gets the <see cref="Anycmd.Snapshots.Providers.ISnapshotProvider"/> instance
        /// that is used for handling the snapshot operations.
        /// </summary>
        public ISnapshotProvider SnapshotProvider
        {
            get { return this._snapshotProvider; }
        }

        #endregion

        #region IDomainRepository Members
        /// <summary>
        /// Gets the instance of the aggregate root with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>The instance of the aggregate root with the specified identifier.</returns>
        public override TAggregateRoot Get<TAggregateRoot>(Guid id)
        {
            var aggregateRoot = this.CreateAggregateRootInstance<TAggregateRoot>();
            if (this._snapshotProvider != null && this._snapshotProvider.HasSnapshot(typeof(TAggregateRoot), id))
            {
                var snapshot = _snapshotProvider.GetSnapshot(typeof(TAggregateRoot), id);
                aggregateRoot.BuildFromSnapshot(snapshot);
                var eventsAfterSnapshot = this._domainEventStorage.LoadEvents(typeof(TAggregateRoot), id, snapshot.Version);
                var historicalEvents = eventsAfterSnapshot as IDomainEvent[] ?? eventsAfterSnapshot.ToArray();
                if (eventsAfterSnapshot != null && historicalEvents.Any())
                    aggregateRoot.BuildFromHistory(historicalEvents);
            }
            else
            {
                aggregateRoot.Id = id;
                var evnts = this._domainEventStorage.LoadEvents(typeof(TAggregateRoot), id);
                var historicalEvents = evnts as IDomainEvent[] ?? evnts.ToArray();
                if (evnts != null && historicalEvents.Any())
                    aggregateRoot.BuildFromHistory(historicalEvents);
                else
                    throw new RepositoryException("The aggregate (id={0}) cannot be found in the domain repository.", id);
            }
            return aggregateRoot;
        }
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get { return _domainEventStorage.DistributedTransactionSupported && base.DistributedTransactionSupported; }
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            base.Rollback();
            _domainEventStorage.Rollback();
            if (this._snapshotProvider != null && this._snapshotProvider.Option == SnapshotProviderOption.Immediate)
            {
                this._snapshotProvider.Rollback();
            }
        }
        #endregion
    }
}
