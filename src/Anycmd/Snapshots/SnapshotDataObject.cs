
namespace Anycmd.Snapshots
{
    using Model;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Util;

    /// <summary>
    /// 数据转移对象（DTO）。表示快照的数据转移对象。
    /// </summary>
    [Serializable]
    [XmlRoot]
    [DataContract]
    public class SnapshotDataObject : IEntity
    {
        #region Ctor
        /// <summary>
        /// 初始化一个 <c>SnapshotDataObject</c> 类型的对象。
        /// </summary>
        public SnapshotDataObject()
        {
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets an array of <see cref="System.Byte"/> values that represents
        /// the binary content of the snapshot data.
        /// </summary>
        [XmlElement]
        [DataMember]
        public byte[] SnapshotData { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the aggregate root.
        /// </summary>
        [XmlElement]
        [DataMember]
        public Guid AggregateRootId { get; set; }
        /// <summary>
        /// Gets or sets the assembly qualified name of the type of the aggregate root.
        /// </summary>
        [XmlElement]
        [DataMember]
        public string AggregateRootType { get; set; }
        /// <summary>
        /// Gets or sets the assembly qualified name of the type of the snapshot.
        /// </summary>
        [XmlElement]
        [DataMember]
        public string SnapshotType { get; set; }
        /// <summary>
        /// Gets or sets the version of the snapshot.
        /// </summary>
        /// <remarks>This version is also equal to the version of the event
        /// on which the snapshot was taken.</remarks>
        [XmlElement]
        [DataMember]
        public long Version { get; set; }
        /// <summary>
        /// Gets or sets the branch of the snapshot.
        /// </summary>
        [XmlElement]
        [DataMember]
        public long Branch { get; set; }
        /// <summary>
        /// Gets or sets the timestamp on which the snapshot was taken.
        /// </summary>
        [XmlElement]
        [DataMember]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Gets or sets the identifier of the snapshot data object.
        /// </summary>
        [XmlElement]
        [DataMember]
        public Guid Id { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the hash code for current snapshot data object.
        /// </summary>
        /// <returns>The calculated hash code for the current snapshot data object.</returns>
        public override int GetHashCode()
        {
            return ReflectionHelper.GetHashCode(this.AggregateRootId.GetHashCode(),
                this.AggregateRootType.GetHashCode(),
                this.Branch.GetHashCode(),
                this.Id.GetHashCode(),
                this.SnapshotData.GetHashCode(),
                this.SnapshotType.GetHashCode(),
                this.Timestamp.GetHashCode(),
                this.Version.GetHashCode());
        }
        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value indicating whether this instance is equal to a specified
        /// object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>True if obj is an instance of the <see cref="Anycmd.Snapshots.SnapshotDataObject"/> type and equals the value of this
        /// instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            var other = obj as SnapshotDataObject;
            if ((object)other == (object)null)
                return false;
            return this.Id == other.Id;
        }
        #endregion
    }
}
