
namespace Anycmd.Snapshots.Providers
{
    using Storage;

    /// <summary>
    /// Represents the snapshot providers that utilize both event storage and snapshot storage to
    /// implement their functionalities.
    /// </summary>
    public abstract class StorageBasedSnapshotProvider : SnapshotProvider
    {
        #region Private Fields
        private readonly IStorage _eventStorage;
        private readonly IStorage _snapshotStorage;
        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of <c>StorageBasedSnapshotProvider</c> class.
        /// </summary>
        /// <param name="eventStorage">The instance of the event storage that is used for initializing the <c>StorageBasedSnapshotProvider</c> class.</param>
        /// <param name="snapshotStorage">The instance of the snapshot storage this is used for initializing the <c>StorageBasedSnapshotProvider</c> class.</param>
        /// <param name="option">The snapshot provider option.</param>
        protected StorageBasedSnapshotProvider(IStorage eventStorage, IStorage snapshotStorage, SnapshotProviderOption option)
            : base(option)
        {
            this._eventStorage = eventStorage;
            this._snapshotStorage = snapshotStorage;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._eventStorage.Dispose();
                this._snapshotStorage.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the instance of the event storage used by <c>StorageBasedSnapshotProvider</c>.
        /// </summary>
        public IStorage EventStorage
        {
            get { return this._eventStorage; }
        }
        /// <summary>
        /// Gets the instance of the snapshot storage used by <c>StorageBasedSnapshotProvider</c>.
        /// </summary>
        public IStorage SnapshotStorage
        {
            get { return this._snapshotStorage; }
        }
        #endregion
    }
}
