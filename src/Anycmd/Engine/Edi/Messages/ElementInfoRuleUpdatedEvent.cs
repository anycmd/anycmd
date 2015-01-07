
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class ElementInfoRuleUpdatedEvent : DomainEvent
    {
        public ElementInfoRuleUpdatedEvent(ElementInfoRule source) : base(source) { }
    }
}
