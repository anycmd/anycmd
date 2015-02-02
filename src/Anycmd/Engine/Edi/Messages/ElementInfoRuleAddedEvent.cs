
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ElementInfoRuleAddedEvent : DomainEvent
    {
        public ElementInfoRuleAddedEvent(IAcSession acSession, ElementInfoRule source) : base(acSession, source) { }
    }
}
