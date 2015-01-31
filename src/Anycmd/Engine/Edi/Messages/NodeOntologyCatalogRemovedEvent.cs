
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeOntologyCatalogRemovedEvent : DomainEvent
    {
        public NodeOntologyCatalogRemovedEvent(IUserSession userSession, NodeOntologyCatalogBase source) : base(userSession, source) { }
    }
}
