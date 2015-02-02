
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class ElementInfoRuleUpdatedEvent : DomainEvent
    {
        public ElementInfoRuleUpdatedEvent(IAcSession acSession, ElementInfoRule source) : base(acSession, source) { }
    }
}
