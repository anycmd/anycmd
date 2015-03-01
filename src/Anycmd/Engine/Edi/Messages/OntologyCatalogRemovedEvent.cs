
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class OntologyCatalogRemovedEvent : DomainEvent
    {
        public OntologyCatalogRemovedEvent(IAcSession acSession, OntologyCatalogBase source) : base(acSession, source) { }

        internal OntologyCatalogRemovedEvent(IAcSession acSession, OntologyCatalogBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
