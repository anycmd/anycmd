
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class OrganizationActionAddedEvent : DomainEvent
    {
        public OrganizationActionAddedEvent(OrganizationAction source) : base(source) { }
    }
}
