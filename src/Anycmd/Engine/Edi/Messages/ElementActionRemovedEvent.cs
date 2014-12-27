
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ElementActionRemovedEvent : DomainEvent
    {
        public ElementActionRemovedEvent(ElementAction source)
            : base(source)
        {
        }
    }
}
