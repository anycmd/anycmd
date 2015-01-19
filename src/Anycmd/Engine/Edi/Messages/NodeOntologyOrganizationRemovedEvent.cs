
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeOntologyOrganizationRemovedEvent : DomainEvent
    {
        public NodeOntologyOrganizationRemovedEvent(IUserSession userSession, NodeOntologyOrganizationBase source) : base(userSession, source) { }
    }
}
