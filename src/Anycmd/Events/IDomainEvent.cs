namespace Anycmd.Events
{
    using Model;

    /// <summary>
    /// 表示该接口的实现类是领域事件。
    /// </summary>
    /// <remarks>领域事件是被领域层引发的事件。</remarks>
    public interface IDomainEvent : IEvent
    {
        /// <summary>
        /// 读取引发该领域事件的业务实体。
        /// </summary>
        IEntity Source { get; }

        /// <summary>
        /// 读取该领域事件的版本号。
        /// </summary>
        long Version { get; }

        /// <summary>
        /// 读取该领域事件所在的分值标识。
        /// </summary>
        long Branch { get; }
    }
}
