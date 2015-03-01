
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class ElementInfoRuleRemovedEvent : DomainEvent
    {
        public ElementInfoRuleRemovedEvent(IAcSession acSession, ElementInfoRule source) : base(acSession, source) { }
    }
}
