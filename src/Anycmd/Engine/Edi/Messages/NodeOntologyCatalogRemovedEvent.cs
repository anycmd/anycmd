
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class NodeOntologyCatalogRemovedEvent : DomainEvent
    {
        public NodeOntologyCatalogRemovedEvent(IAcSession acSession, NodeOntologyCatalogBase source) : base(acSession, source) { }

        internal NodeOntologyCatalogRemovedEvent(IAcSession acSession, NodeOntologyCatalogBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
