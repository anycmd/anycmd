
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeCatalogActionRemovedEvent : DomainEvent
    {
        public NodeCatalogActionRemovedEvent(IUserSession userSession, NodeCatalogAction source) : base(userSession, source) { }
    }
}
