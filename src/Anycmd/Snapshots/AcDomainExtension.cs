
namespace Anycmd.Snapshots
{
    using Model;
    using Serialization;
    using System;
    using Util;

    public static class AcDomainExtension
    {
        /// <summary>
        /// 从给定的快照数据对象萃取得到快照对象。
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="dataObject">快照数据对象</param>
        /// <returns>快照对象</returns>
        public static ISnapshot ExtractSnapshot(this IAcDomain acDomain, SnapshotDataObject dataObject)
        {
            var serializer = acDomain.RetrieveRequiredService<ISnapshotSerializer>();
            var snapshotType = Type.GetType(dataObject.SnapshotType);
            if (snapshotType == null)
                return null;

            return (ISnapshot)serializer.Deserialize(snapshotType, dataObject.SnapshotData);
        }

        /// <summary>
        /// 为给定的聚合根实体创建快照数据对象。
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="aggregateRoot">快照为该聚合根对象创建。</param>
        /// <returns>快照数据对象。</returns>
        public static SnapshotDataObject CreateFromAggregateRoot(this IAcDomain acDomain, ISourcedAggregateRoot aggregateRoot)
        {
            var serializer = acDomain.RetrieveRequiredService<ISnapshotSerializer>();

            var snapshot = aggregateRoot.CreateSnapshot();

            return new SnapshotDataObject
            {
                AggregateRootId = aggregateRoot.Id,
                AggregateRootType = aggregateRoot.GetType().AssemblyQualifiedName,
                Version = aggregateRoot.Version,
                Branch = Constants.ApplicationRuntime.DefaultBranch,
                SnapshotType = snapshot.GetType().AssemblyQualifiedName,
                Timestamp = snapshot.Timestamp,
                SnapshotData = serializer.Serialize(snapshot)
            };
        }
    }
}
