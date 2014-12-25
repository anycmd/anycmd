
namespace Anycmd.Snapshots
{
    using System;

    /// <summary>
    /// 表示快照。
    /// </summary>
    [Serializable]
    public abstract class Snapshot : ISnapshot
    {
        #region Ctor
        /// <summary>
        /// 初始化一个 <c>Snapshot</c> 类型的对象。
        /// </summary>
        protected Snapshot() { }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the version of the snapshot.
        /// </summary>
        public long Version { get; set; }
        /// <summary>
        /// Gets or sets the branch of the snapshot.
        /// </summary>
        public long Branch { get; set; }
        /// <summary>
        /// Gets or sets the timestamp on which the snapshot was taken.
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the aggregate root which the
        /// snapshot represents.
        /// </summary>
        public Guid AggregateRootId { get; set; }
        #endregion
    }
}
