
namespace Anycmd.Snapshots.Providers
{
    using Model;
    using System;

    /// <summary>
    /// Represents the snapshot provider, when it is used, suppresses
    /// any snapshot functionalities.
    /// </summary>
    public sealed class SuppressedSnapshotProvider : SnapshotProvider
    {
        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>SuppressedSnapshotProvider</c> class.
        /// </summary>
        public SuppressedSnapshotProvider() : base(SnapshotProviderOption.Postpone) { }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets a <see cref="System.Boolean"/> value which indicates
        /// whether the Unit of Work could support Microsoft Distributed
        /// Transaction Coordinator (MS-DTC).
        /// </summary>
        public override bool DistributedTransactionSupported
        {
            get { return false; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value which indicates
        /// whether the snapshot should be created or updated for the given
        /// aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <returns>True if the snapshot should be created or updated, 
        /// otherwise false.</returns>
        /// <remarks>This method always returns false in<c>SuppressedSnapshotProvider</c>
        /// to prevent the snapshots being created or updated.</remarks>
        public override bool CanCreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot)
        {
            return false;
        }
        /// <summary>
        /// Creates or updates the snapshot for the given aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root on which the snapshot is created or updated.</param>
        /// <remarks>For <c>SuppressedSnapshotProvider</c>, nothing is done in this method.</remarks>
        public override void CreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot) { }
        /// <summary>
        /// Gets the snapshot for the aggregate root with the given type and identifier.
        /// </summary>
        /// <param name="aggregateRootType">The type of the aggregate root.</param>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>The snapshot instance.</returns>
        /// <remarks>For <c>SuppressedSnapshotProvider</c>, null reference is returned simply indicating
        /// that no snapshot object is being loaded.</remarks>
        public override ISnapshot GetSnapshot(Type aggregateRootType, Guid id)
        {
            return null;
        }
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value which indicates whether the snapshot
        /// exists for the aggregate root with the given type and identifier.
        /// </summary>
        /// <param name="aggregateRootType">The type of the aggregate root.</param>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>True if the snapshot exists, otherwise false.</returns>
        /// <remarks>This method always returns false in <c>SuppressedSnapshotProvider</c>
        /// indicating that, for any aggregate root, there is no snapshot in the system.</remarks>
        public override bool HasSnapshot(Type aggregateRootType, Guid id)
        {
            return false;
        }
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public override void Commit() { }
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public override void Rollback() { }
        #endregion
    }
}
