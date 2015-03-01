
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class NodeCatalogActionRemovedEvent : DomainEvent
    {
        public NodeCatalogActionRemovedEvent(IAcSession acSession, NodeCatalogAction source) : base(acSession, source) { }
    }
}
