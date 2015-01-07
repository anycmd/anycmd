
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class AccountUpdatedEvent : DomainEvent
    {
        public AccountUpdatedEvent(AccountBase source) : base(source) { }
    }
}