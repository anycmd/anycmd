
namespace Anycmd.Engine.Edi.Messages
{
    using Events;
    using Abstractions;

    public class OntologyOrganizationRemovedEvent : DomainEvent
    {
        public OntologyOrganizationRemovedEvent(OntologyOrganizationBase source) : base(source) { }
    }
}
