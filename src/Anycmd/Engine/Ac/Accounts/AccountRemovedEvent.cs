
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    public sealed class AccountRemovedEvent : DomainEvent
    {
        public AccountRemovedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}