
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class OrganizationActionAddedEvent : DomainEvent
    {
        public OrganizationActionAddedEvent(IUserSession userSession, OrganizationAction source) : base(userSession, source) { }
    }
}
