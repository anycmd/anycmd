
namespace Anycmd.Engine.Edi.Messages
{
    using Anycmd.Events;
    using Engine.Edi.Abstractions;

    public class ElementInfoRuleUpdatedEvent : DomainEvent
    {
        public ElementInfoRuleUpdatedEvent(ElementInfoRule source) : base(source) { }
    }
}
