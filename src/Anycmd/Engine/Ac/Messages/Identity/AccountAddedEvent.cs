
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    public class AccountAddedEvent : DomainEvent
    {
        public AccountAddedEvent(AccountBase source) : base(source) { }
    }
}