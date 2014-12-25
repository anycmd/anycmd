namespace Anycmd.Events
{
    /// <summary>
    /// 表示领域事件处理器。
    /// </summary>
    /// <typeparam name="TDomainEvent">被当前领域事件处理器处理的领域事件.NET类型。</typeparam>
    public interface IDomainEventHandler<in TDomainEvent> : IEventHandler<TDomainEvent>
        where TDomainEvent : class, IDomainEvent
    {

    }
}
