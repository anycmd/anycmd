
namespace Anycmd.Engine.Edi.Messages
{
    using Anycmd.Events;
    using Engine.Edi.Abstractions;

    public class ElementInfoRuleRemovedEvent : DomainEvent {
        public ElementInfoRuleRemovedEvent(ElementInfoRule source) : base(source) { }
    }
}
