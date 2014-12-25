
namespace Anycmd.Bus.AggregatorBus
{
    using Events;

    /// <summary>
    /// 表示使用聚合器实现的命令总线。
    /// </summary>
    public sealed class AggregatorCommandBus : AggregatorBus, ICommandBus
    {
        #region Ctor
        /// <summary>
        /// 实例化一个 <c>AggregatorCommandBus</c> 类型的对象。
        /// </summary>
        public AggregatorCommandBus(IEventAggregator eventAggregator) : base(eventAggregator) { }
        #endregion
    }
}
