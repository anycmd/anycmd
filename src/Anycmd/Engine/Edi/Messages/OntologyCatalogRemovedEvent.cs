
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class OntologyCatalogRemovedEvent : DomainEvent
    {
        public OntologyCatalogRemovedEvent(IUserSession userSession, OntologyCatalogBase source) : base(userSession, source) { }
    }
}
