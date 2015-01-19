
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class ElementInfoRuleAddedEvent : DomainEvent
    {
        public ElementInfoRuleAddedEvent(IUserSession userSession, ElementInfoRule source) : base(userSession, source) { }
    }
}
