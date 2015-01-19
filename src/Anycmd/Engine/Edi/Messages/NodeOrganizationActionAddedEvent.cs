
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeOrganizationActionAddedEvent : DomainEvent
    {
        public NodeOrganizationActionAddedEvent(IUserSession userSession, NodeOrganizationAction source) : base(userSession, source) { }
    }
}
