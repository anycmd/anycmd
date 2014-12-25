
namespace Anycmd.Engine.Host.Edi.Messages
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
