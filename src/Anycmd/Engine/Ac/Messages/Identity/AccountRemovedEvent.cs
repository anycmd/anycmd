
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class AccountRemovedEvent : DomainEvent
    {
        public AccountRemovedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}