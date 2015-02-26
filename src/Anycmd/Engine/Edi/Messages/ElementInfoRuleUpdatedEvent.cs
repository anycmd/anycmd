
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ElementInfoRuleUpdatedEvent : DomainEvent
    {
        public ElementInfoRuleUpdatedEvent(IAcSession acSession, ElementInfoRule source) : base(acSession, source) { }
    }
}
