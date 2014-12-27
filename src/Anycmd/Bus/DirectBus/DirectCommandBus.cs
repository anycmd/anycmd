
namespace Anycmd.Bus.DirectBus
{
    /// <summary>
    /// 表示当命令总线提交（Commit）时命令将被立即分发的命令总线。
    /// </summary>
    public sealed class DirectCommandBus : DirectBus, ICommandBus
    {
        /// <summary>
        /// 初始化一个 <c>DirectCommandBus</c> 类型的对象。
        /// </summary>
        /// <param name="dispatcher">总线中所使用的消息分发器对象 <see cref="Anycmd.Bus.IMessageDispatcher"/>。</param>
        public DirectCommandBus(IMessageDispatcher dispatcher) : base(dispatcher) { }
    }
}
