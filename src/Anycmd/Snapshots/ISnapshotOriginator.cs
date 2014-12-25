
namespace Anycmd.Snapshots
{
    /// <summary>
    /// 表示该接口的实现类是快照发起人。
    /// </summary>
    public interface ISnapshotOriginator
    {
        /// <summary>
        /// 通过给定的快照建造当前快照发起人。
        /// </summary>
        /// <param name="snapshot">给定的快照，当前快照发起人从它构建。</param>
        void BuildFromSnapshot(ISnapshot snapshot);

        /// <summary>
        /// 创建一个快照。
        /// </summary>
        /// <returns>基于当前快照发起人构建的快照。</returns>
        ISnapshot CreateSnapshot();
    }
}
