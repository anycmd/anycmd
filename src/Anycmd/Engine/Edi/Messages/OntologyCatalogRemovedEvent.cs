
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class OntologyCatalogRemovedEvent : DomainEvent
    {
        public OntologyCatalogRemovedEvent(IAcSession userSession, OntologyCatalogBase source) : base(userSession, source) { }
    }
}
