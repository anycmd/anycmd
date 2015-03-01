
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public sealed class OntologyCatalogRemovedEvent : DomainEvent
    {
        public OntologyCatalogRemovedEvent(IAcSession acSession, OntologyCatalogBase source) : base(acSession, source) { }

        internal bool IsPrivate { get; set; }
    }
}
