
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class NodeOntologyCatalogRemovedEvent : DomainEvent
    {
        public NodeOntologyCatalogRemovedEvent(IAcSession acSession, NodeOntologyCatalogBase source) : base(acSession, source) { }

        internal bool IsPrivate { get; set; }
    }
}
