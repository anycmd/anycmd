
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementActionAddedEvent : DomainEvent
    {
        public ElementActionAddedEvent(ElementAction source)
            : base(source)
        {
        }
    }
}
