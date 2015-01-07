
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class OrganizationActionUpdatedEvent : DomainEvent
    {
        public OrganizationActionUpdatedEvent(OrganizationAction source)
            : base(source)
        {
        }
    }
}
