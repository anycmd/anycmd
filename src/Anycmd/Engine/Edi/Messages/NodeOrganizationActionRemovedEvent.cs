
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeOrganizationActionRemovedEvent : DomainEvent
    {
        public NodeOrganizationActionRemovedEvent(IUserSession userSession, NodeOrganizationAction source) : base(userSession, source) { }
    }
}
