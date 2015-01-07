
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ElementInfoRuleAddedEvent : DomainEvent
    {
        public ElementInfoRuleAddedEvent(ElementInfoRule source) : base(source) { }
    }
}
