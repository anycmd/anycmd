
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class OntologyOrganizationRemovedEvent : DomainEvent
    {
        public OntologyOrganizationRemovedEvent(IUserSession userSession, OntologyOrganizationBase source) : base(userSession, source) { }
    }
}
