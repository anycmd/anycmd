
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class OntologyCatalogRemovedEvent : DomainEvent
    {
        public OntologyCatalogRemovedEvent(IAcSession acSession, OntologyCatalogBase source) : base(acSession, source) { }
    }
}
