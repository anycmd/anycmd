
namespace Anycmd.Snapshots.Providers
{
    using Model;
    using System;

    /// <summary>
    /// 表示所有快照提供程序的基类。
    /// </summary>
    public abstract class SnapshotProvider : DisposableObject, ISnapshotProvider
    {
        #region Private Fields
        private readonly SnapshotProviderOption _option;

        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>SnapshotProvider</c> class.
        /// </summary>
        /// <param name="option">The <see cref="Anycmd.Snapshots.Providers.SnapshotProviderOption"/> value
        /// which is used for initializing the <c>SnapshotProvider</c> class.</param>
        protected SnapshotProvider(SnapshotProviderOption option)
        {
            this._option = option;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing) { }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets a <see cref="Anycmd.Snapshots.Providers.SnapshotProviderOption"/> value
        /// which indicates the option when using the snapshot functionalities.
        /// </summary>
        public SnapshotProviderOption Option
        {
            get { return this._option; }
        }
        #endregion

        #region ISnapshotProvider Members
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value which indicates
        /// whether the snapshot should be created or updated for the given
        /// aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <returns>True if the snapshot should be created or updated, 
        /// otherwise false.</returns>
        public abstract bool CanCreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot);
        /// <summary>
        /// Creates or updates the snapshot for the given aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root on which the snapshot is created or updated.</param>
        public abstract void CreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot);
        /// <summary>
        /// Gets the snapshot for the aggregate root with the given type and identifier.
        /// </summary>
        /// <param name="aggregateRootType">The type of the aggregate root.</param>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>The snapshot instance.</returns>
        public abstract ISnapshot GetSnapshot(Type aggregateRootType, Guid id);
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value which indicates whether the snapshot
        /// exists for the aggregate root with the given type and identifier.
        /// </summary>
        /// <param name="aggregateRootType">The type of the aggregate root.</param>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>True if the snapshot exists, otherwise false.</returns>
        public abstract bool HasSnapshot(Type aggregateRootType, Guid id);
        #endregion

        #region IUnitOfWork Members
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public abstract bool DistributedTransactionSupported { get; }

        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work was successfully committed.
        /// </summary>
        public bool Committed { get; protected set; }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public abstract void Commit();
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public abstract void Rollback();
        #endregion
    }
}
