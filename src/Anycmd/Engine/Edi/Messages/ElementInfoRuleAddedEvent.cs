
namespace Anycmd.Engine.Edi.Messages
{
    using Anycmd.Events;
    using Engine.Edi.Abstractions;

    public class ElementInfoRuleAddedEvent : DomainEvent
    {
        public ElementInfoRuleAddedEvent(ElementInfoRule source) : base(source) { }
    }
}
