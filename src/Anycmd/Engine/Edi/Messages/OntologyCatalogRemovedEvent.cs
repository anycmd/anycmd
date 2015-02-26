
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class OntologyCatalogRemovedEvent : DomainEvent
    {
        public OntologyCatalogRemovedEvent(IAcSession acSession, OntologyCatalogBase source) : base(acSession, source) { }
    }
}
