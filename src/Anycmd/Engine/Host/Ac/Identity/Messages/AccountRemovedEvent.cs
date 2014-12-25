
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    public class AccountRemovedEvent : DomainEvent
    {
        public AccountRemovedEvent(AccountBase source) : base(source) { }
    }
}