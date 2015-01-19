
namespace Anycmd.Engine.Edi.Messages
{
    using Abstractions;
    using Events;

    public class OrganizationActionRemovedEvent : DomainEvent
    {
        public OrganizationActionRemovedEvent(IUserSession userSession, OrganizationAction source) : base(userSession, source) { }
    }
}
