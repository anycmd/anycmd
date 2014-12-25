
namespace Anycmd.Snapshots.Providers
{
    /// <summary>
    /// 表示快照提供程序的可选项。
    /// </summary>
    public enum SnapshotProviderOption
    {
        /// <summary>
        /// 表示快照的create/update应该被立即执行。
        /// </summary>
        Immediate,
        /// <summary>
        /// 表示快照的creating/updating可以被推迟到以后的场景。 
        /// </summary>
        Postpone
    }
}
