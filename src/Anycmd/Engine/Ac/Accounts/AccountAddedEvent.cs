
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    public class AccountAddedEvent : DomainEvent
    {
        public AccountAddedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}