
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    public class AccountRemovedEvent : DomainEvent
    {
        public AccountRemovedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}