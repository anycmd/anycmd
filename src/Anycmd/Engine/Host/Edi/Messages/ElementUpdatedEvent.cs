
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementUpdatedEvent : DomainEvent
    {
        public ElementUpdatedEvent(ElementBase source)
            : base(source)
        {
        }
    }
}
