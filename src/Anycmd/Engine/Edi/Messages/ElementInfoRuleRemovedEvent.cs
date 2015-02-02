
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class ElementInfoRuleRemovedEvent : DomainEvent
    {
        public ElementInfoRuleRemovedEvent(IAcSession acSession, ElementInfoRule source) : base(acSession, source) { }
    }
}
