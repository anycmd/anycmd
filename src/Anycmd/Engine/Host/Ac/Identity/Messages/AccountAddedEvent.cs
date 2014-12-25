
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    public class AccountAddedEvent : DomainEvent
    {
        public AccountAddedEvent(AccountBase source) : base(source) { }
    }
}