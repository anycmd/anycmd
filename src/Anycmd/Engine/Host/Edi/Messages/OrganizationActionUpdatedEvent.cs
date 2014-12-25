
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Engine.Edi.Abstractions;
    using Events;

    public class OrganizationActionUpdatedEvent : DomainEvent
    {
        public OrganizationActionUpdatedEvent(OrganizationAction source)
            : base(source)
        {
        }
    }
}
