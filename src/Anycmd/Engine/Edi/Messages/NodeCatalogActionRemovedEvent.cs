
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class NodeCatalogActionRemovedEvent : DomainEvent
    {
        public NodeCatalogActionRemovedEvent(IAcSession acSession, NodeCatalogAction source) : base(acSession, source) { }
    }
}
