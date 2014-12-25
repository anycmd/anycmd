
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    public class AccountUpdatedEvent : DomainEvent
    {
        public AccountUpdatedEvent(AccountBase source) : base(source) { }
    }
}