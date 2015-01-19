
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class AccountUpdatedEvent : DomainEvent
    {
        public AccountUpdatedEvent(IUserSession userSession, AccountBase source) : base(userSession, source) { }
    }
}