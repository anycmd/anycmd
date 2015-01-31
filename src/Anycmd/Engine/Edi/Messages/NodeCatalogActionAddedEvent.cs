
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeCatalogActionAddedEvent : DomainEvent
    {
        public NodeCatalogActionAddedEvent(IUserSession userSession, NodeCatalogAction source) : base(userSession, source) { }
    }
}
