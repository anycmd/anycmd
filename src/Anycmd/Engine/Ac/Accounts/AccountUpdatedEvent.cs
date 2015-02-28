
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    public sealed class AccountUpdatedEvent : DomainEvent
    {
        public AccountUpdatedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}