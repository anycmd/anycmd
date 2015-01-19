
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class AccountAddedEvent : DomainEvent
    {
        public AccountAddedEvent(IUserSession userSession, AccountBase source) : base(userSession, source) { }
    }
}