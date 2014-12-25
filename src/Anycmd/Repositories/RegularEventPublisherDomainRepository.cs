
namespace Anycmd.Repositories
{
    using Bus;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    /// <summary>
    /// Represents the domain repository that uses the <see cref="Anycmd.Repositories.IRepositoryContext"/>
    /// and <see cref="Anycmd.Repositories.IRepository&lt;TAggregateRoot&gt;"/> instances to perform aggregate
    /// operations and publishes the domain events to the specified event bus.
    /// </summary>
    public class RegularEventPublisherDomainRepository : EventPublisherDomainRepository
    {
        #region Private Fields
        private readonly IRepositoryContext _context;
        private readonly HashSet<ISourcedAggregateRoot> _dirtyHash = new HashSet<ISourcedAggregateRoot>();
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>RegularEventPublisherDomainRepository</c> class.
        /// </summary>
        /// <param name="context">The <see cref="Anycmd.Repositories.IRepositoryContext"/>instance
        /// that is used by the current domain repository to perform aggregate operations.</param>
        /// <param name="eventBus">The event bus to which the domain events are published.</param>
        public RegularEventPublisherDomainRepository(IRepositoryContext context, IEventBus eventBus)
            : base(eventBus)
        {
            this._context = context;
        }
        #endregion

        #region Private Methods
        private void PublishAggregateRootEvents(ISourcedAggregateRoot aggregateRoot)
        {
            foreach (var evt in aggregateRoot.UncommittedEvents)
            {
                this.EventBus.Publish(evt);
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Commits the changes registered in the domain repository.
        /// </summary>
        protected override void DoCommit()
        {
            foreach (var aggregateRootObj in this.SaveHash)
            {
                this._context.RegisterNew(aggregateRootObj);
                this.PublishAggregateRootEvents(aggregateRootObj);
            }
            foreach (var aggregateRootObj in this._dirtyHash)
            {
                this._context.RegisterModified(aggregateRootObj);
                this.PublishAggregateRootEvents(aggregateRootObj);
            }
            if (this.DistributedTransactionSupported)
            {
                using (var ts = new TransactionScope())
                {
                    this._context.Commit();
                    this.EventBus.Commit();
                    ts.Complete();
                }
            }
            else
            {
                this._context.Commit();
                this.EventBus.Commit();
            }
            this._dirtyHash.ToList().ForEach(this.DelegatedUpdateAndClearAggregateRoot);
            this._dirtyHash.Clear();
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
                this._context.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the <see cref="Anycmd.Repositories.IRepositoryContext"/>instance
        /// that is used by the current domain repository to perform aggregate operations.
        /// </summary>
        public IRepositoryContext Context
        {
            get { return this._context; }
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
            var querySaveHash = from p in this.SaveHash
                                where p.Id.Equals(id)
                                select p;
            var queryDirtyHash = from p in this._dirtyHash
                                 where p.Id.Equals(id)
                                 select p;
            var sourcedAggregateRoots = querySaveHash as ISourcedAggregateRoot[] ?? querySaveHash.ToArray();
            if (sourcedAggregateRoots.Any())
                return sourcedAggregateRoots.FirstOrDefault() as TAggregateRoot;
            var aggregateRoots = queryDirtyHash as ISourcedAggregateRoot[] ?? queryDirtyHash.ToArray();
            if (aggregateRoots.Any())
                return aggregateRoots.FirstOrDefault() as TAggregateRoot;

            var result = _context.Query<TAggregateRoot>().FirstOrDefault(ar => ar.Id.Equals(id));
            // Clears the aggregate root since version info is not needed in regular repositories.
            this.DelegatedUpdateAndClearAggregateRoot(result);
            return result;
        }
        /// <summary>
        /// Saves the aggregate represented by the specified aggregate root to the repository.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root that is going to be saved.</param>
        public override void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
        {
            if (_context.Query<TAggregateRoot>().Any(ar => ar.Id.Equals(aggregateRoot.Id)))
            {
                if (!this._dirtyHash.Contains(aggregateRoot))
                    this._dirtyHash.Add(aggregateRoot);
                this.Committed = false;
            }
            else
            {
                base.Save<TAggregateRoot>(aggregateRoot);
            }
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
            get
            {
                return this._context.DistributedTransactionSupported && base.DistributedTransactionSupported;
            }
        }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback()
        {
            base.Rollback();
            this._context.Rollback();
        }
        #endregion
    }
}
