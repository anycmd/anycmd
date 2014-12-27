
namespace Anycmd.Engine.Edi.Messages
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
