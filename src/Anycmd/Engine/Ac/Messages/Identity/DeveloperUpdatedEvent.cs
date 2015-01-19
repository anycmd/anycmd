
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class DeveloperUpdatedEvent : DomainEvent
    {
        public DeveloperUpdatedEvent(IUserSession userSession, AccountBase source) : base(userSession, source) { }
    }
}
