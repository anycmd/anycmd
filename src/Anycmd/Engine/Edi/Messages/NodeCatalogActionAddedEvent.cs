
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class NodeCatalogActionAddedEvent : DomainEvent
    {
        public NodeCatalogActionAddedEvent(IAcSession acSession, NodeCatalogAction source) : base(acSession, source) { }
    }
}
