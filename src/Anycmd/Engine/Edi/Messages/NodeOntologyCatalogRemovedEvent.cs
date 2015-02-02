
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeOntologyCatalogRemovedEvent : DomainEvent
    {
        public NodeOntologyCatalogRemovedEvent(IAcSession acSession, NodeOntologyCatalogBase source) : base(acSession, source) { }
    }
}
