
namespace Anycmd.Bus.AggregatorBus
{
    using Events;

    /// <summary>
    /// 表示使用聚合器实现的命令总线。
    /// </summary>
    public sealed class AggregatorEventBus : AggregatorBus, IEventBus
    {
        #region Ctor
        /// <summary>
        /// 实例化一个 <c>AggregatorEventBus</c> 类型的对象。
        /// </summary>
        public AggregatorEventBus(IEventAggregator eventAggregator) : base(eventAggregator) { }
        #endregion
    }
}
