
namespace Anycmd.Engine.Edi.Messages
{
    using Anycmd.Events;
    using Engine.Edi.Abstractions;

    public class OntologyOrganizationRemovedEvent : DomainEvent
    {
        public OntologyOrganizationRemovedEvent(OntologyOrganizationBase source) : base(source) { }
    }
}
