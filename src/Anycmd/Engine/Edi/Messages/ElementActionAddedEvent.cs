
namespace Anycmd.Engine.Edi.Messages
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
