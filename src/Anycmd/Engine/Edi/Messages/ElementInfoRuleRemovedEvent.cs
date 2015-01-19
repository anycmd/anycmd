
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class ElementInfoRuleRemovedEvent : DomainEvent
    {
        public ElementInfoRuleRemovedEvent(IUserSession userSession, ElementInfoRule source) : base(userSession, source) { }
    }
}
