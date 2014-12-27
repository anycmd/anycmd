
namespace Anycmd.Bus.DirectBus
{
    /// <summary>
    /// 表示当事件总线提交（Commit）时事件将被立即分发的事件总线。
    /// </summary>
    public sealed class DirectEventBus : DirectBus, IEventBus
    {
        /// <summary>
        /// 初始化一个 <c>DirectEventBus</c> 类型的对象。
        /// </summary>
        /// <param name="dispatcher">总线中所使用的消息分发器对象 <see cref="Anycmd.Bus.IMessageDispatcher"/>。</param>
        public DirectEventBus(IMessageDispatcher dispatcher) : base(dispatcher) { }
    }
}
