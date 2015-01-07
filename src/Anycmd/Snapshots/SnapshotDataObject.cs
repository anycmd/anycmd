
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
        /// <summary>
        /// 初始化一个 <c>SnapshotDataObject</c> 类型的对象。
        /// </summary>
        public SnapshotDataObject()
        {
        }

        #region Public Properties
        /// <summary>
        /// 读取或设置表示快照数据的<see cref="System.Byte"/>数组。
        /// </summary>
        [XmlElement]
        [DataMember]
        public byte[] SnapshotData { get; set; }

        /// <summary>
        /// 读取或设置聚合根标识
        /// </summary>
        [XmlElement]
        [DataMember]
        public Guid AggregateRootId { get; set; }

        /// <summary>
        /// 读取或设置程序集级唯一的快照聚合根对象类型
        /// </summary>
        [XmlElement]
        [DataMember]
        public string AggregateRootType { get; set; }

        /// <summary>
        /// 读取或设置程序集级唯一的快照对象类型
        /// </summary>
        [XmlElement]
        [DataMember]
        public string SnapshotType { get; set; }

        /// <summary>
        /// 读取或设置快照的版本
        /// </summary>
        /// <remarks>该值与该快照拍摄时所在的事件对象的version相等。</remarks>
        [XmlElement]
        [DataMember]
        public long Version { get; set; }

        /// <summary>
        /// 读取或设置分支标识。
        /// </summary>
        [XmlElement]
        [DataMember]
        public long Branch { get; set; }

        /// <summary>
        /// 读取或设置快照拍摄时间戳
        /// </summary>
        [XmlElement]
        [DataMember]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 读取或设置快照数据对象标识
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
