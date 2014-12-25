
namespace Anycmd.Repositories
{
    using Bus;
    using Snapshots;
    using Specifications;
    using Storage;
    using System;
    using System.Transactions;


    /// <summary>
    /// Represents the domain repository that uses the snapshots to perform
    /// repository operations and publishes the domain events to the specified
    /// event bus.
    /// </summary>
    public class SnapshotDomainRepository : EventPublisherDomainRepository
    {
        #region Private Fields
        private readonly IStorage _storage;
        private readonly IAcDomain _host;
        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of <c>SnapshotDomainRepository</c> class.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="storage">The <see cref="Anycmd.Storage.IStorage"/> instance that is used
        /// by the current domain repository to manipulate snapshot data.</param>
        /// <param name="eventBus">The <see cref="Anycmd.Bus.IEventBus"/> instance to which
        /// the domain events are published.</param>
        public SnapshotDomainRepository(IAcDomain host, IStorage storage, IEventBus eventBus)
            : base(eventBus)
        {
            this._host = host;
            this._storage = storage;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Commits the changes registered in the domain repository.
        /// </summary>
        protected override void DoCommit()
        {
            foreach (var aggregateRoot in this.SaveHash)
            {
                var snapshotDataObject = _host.CreateFromAggregateRoot(aggregateRoot);
                var aggregateRootId = aggregateRoot.Id;
                var aggregateRootType = aggregateRoot.GetType().AssemblyQualifiedName;
                ISpecification<SnapshotDataObject> spec = Specification<SnapshotDataObject>.Eval(p => p.AggregateRootId == aggregateRootId && p.AggregateRootType == aggregateRootType);
                var firstMatch = this._storage.SelectFirstOnly<SnapshotDataObject>(spec);
                if (firstMatch != null)
                    this._storage.Update<SnapshotDataObject>(new PropertyBag(snapshotDataObject), spec);
                else
                    this._storage.Insert<SnapshotDataObject>(new PropertyBag(snapshotDataObject));
                foreach (var evnt in aggregateRoot.UncommittedEvents)
                {
                    this.EventBus.Publish(evnt);
                }
            }
            if (this.DistributedTransactionSupported)
            {
                using (var ts = new TransactionScope())
                {
                    this._storage.Commit();
                    this.EventBus.Commit();
                    ts.Complete();
                }
            }
            else
            {
                this._storage.Commit();
                this.EventBus.Commit();
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
                this._storage.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the <see cref="Anycmd.Storage.IStorage"/> instance that is used
        /// by the current domain repository to manipulate snapshot data.
        /// </summary>
        public IStorage Storage
        {
            get { return this._storage; }
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
            var aggregateRootType = typeof(TAggregateRoot).AssemblyQualifiedName;
            ISpecification<SnapshotDataObject> spec = Specification<SnapshotDataObject>.Eval(p => p.AggregateRootId == id && p.AggregateRootType == aggregateRootType);
            var snapshotDataObject = this._storage.SelectFirstOnly<SnapshotDataObject>(spec);
            if (snapshotDataObject == null)
                throw new RepositoryException("The aggregate (id={0}) cannot be found in the domain repository.", id);
            var snapshot = _host.ExtractSnapshot(snapshotDataObject);
            var aggregateRoot = this.CreateAggregateRootInstance<TAggregateRoot>();
            aggregateRoot.BuildFromSnapshot(snapshot);
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
            get { return this._storage.DistributedTransactionSupported && base.DistributedTransactionSupported; }
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            base.Rollback();
            this._storage.Rollback();
        }
        #endregion
    }
}
