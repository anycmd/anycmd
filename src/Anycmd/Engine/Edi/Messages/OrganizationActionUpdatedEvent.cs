
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class OrganizationActionUpdatedEvent : DomainEvent
    {
        public OrganizationActionUpdatedEvent(IUserSession userSession, OrganizationAction source)
            : base(userSession, source)
        {
        }
    }
}
