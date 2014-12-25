
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class NodeOntologyOrganizationRemovedEvent : DomainEvent
    {
        public NodeOntologyOrganizationRemovedEvent(NodeOntologyOrganizationBase source) : base(source) { }
    }
}
