
namespace Anycmd.Snapshots
{
    using System;

    /// <summary>
    /// 表示该接口的实现类是快照。
    /// </summary>
    public interface ISnapshot
    {
        /// <summary>
        /// Gets or sets the timestamp on which the snapshot was taken.
        /// </summary>
        DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the aggregate root which the
        /// snapshot represents.
        /// </summary>
        Guid AggregateRootId { get; set; }
        /// <summary>
        /// Gets or sets the version of the snapshot, which commonly would
        /// be the version of the aggregate root.
        /// </summary>
        long Version { get; set; }
        /// <summary>
        /// Gets or sets the branch on which the snapshot exists.
        /// </summary>
        long Branch { get; set; }
    }
}
