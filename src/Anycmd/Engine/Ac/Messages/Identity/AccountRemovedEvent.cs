
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    public class AccountRemovedEvent : DomainEvent
    {
        public AccountRemovedEvent(AccountBase source) : base(source) { }
    }
}