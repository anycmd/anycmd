
namespace Anycmd.Snapshots.Providers
{
    using Model;
    using System;

    /// <summary>
    /// 表示该接口的实现类是快照提供程序。
    /// </summary>
    public interface ISnapshotProvider : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value which indicates
        /// whether the snapshot should be created or updated for the given
        /// aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root.</param>
        /// <returns>True if the snapshot should be created or updated, 
        /// otherwise false.</returns>
        bool CanCreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot);

        /// <summary>
        /// Creates or updates the snapshot for the given aggregate root.
        /// </summary>
        /// <param name="aggregateRoot">The aggregate root on which the snapshot is created or updated.</param>
        void CreateOrUpdateSnapshot(ISourcedAggregateRoot aggregateRoot);

        /// <summary>
        /// Gets the snapshot for the aggregate root with the given type and identifier.
        /// </summary>
        /// <param name="aggregateRootType">The type of the aggregate root.</param>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>The snapshot instance.</returns>
        ISnapshot GetSnapshot(Type aggregateRootType, Guid id);

        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value which indicates whether the snapshot
        /// exists for the aggregate root with the given type and identifier.
        /// </summary>
        /// <param name="aggregateRootType">The type of the aggregate root.</param>
        /// <param name="id">The identifier of the aggregate root.</param>
        /// <returns>True if the snapshot exists, otherwise false.</returns>
        bool HasSnapshot(Type aggregateRootType, Guid id);

        /// <summary>
        /// Gets a <see cref="Anycmd.Snapshots.Providers.SnapshotProviderOption"/> value
        /// which indicates the option when using the snapshot functionalities.
        /// </summary>
        SnapshotProviderOption Option { get; }
    }
}
