
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class OrganizationActionAddedEvent : DomainEvent
    {
        public OrganizationActionAddedEvent(OrganizationAction source) : base(source) { }
    }
}
