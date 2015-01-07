
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeOntologyOrganizationRemovedEvent : DomainEvent
    {
        public NodeOntologyOrganizationRemovedEvent(NodeOntologyOrganizationBase source) : base(source) { }
    }
}
