
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class OrganizationActionAddedEvent : DomainEvent
    {
        public OrganizationActionAddedEvent(OrganizationAction source) : base(source) { }
    }
}
