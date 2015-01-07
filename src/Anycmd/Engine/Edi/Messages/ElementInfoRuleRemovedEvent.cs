
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class ElementInfoRuleRemovedEvent : DomainEvent
    {
        public ElementInfoRuleRemovedEvent(ElementInfoRule source) : base(source) { }
    }
}
