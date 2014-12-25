
namespace Anycmd.Events
{
    using Bus;

    /// <summary>
    /// 表示该接口的实现类是事件处理器。
    /// </summary>
    /// <typeparam name="TEvent">被处理的事件的类型。</typeparam>
    [RegisterDispatch]
    public interface IEventHandler<in TEvent> : IHandler<TEvent>
        where TEvent : class, IEvent
    {
    }
}
