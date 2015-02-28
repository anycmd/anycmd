
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    public sealed class AccountAddedEvent : DomainEvent
    {
        public AccountAddedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}