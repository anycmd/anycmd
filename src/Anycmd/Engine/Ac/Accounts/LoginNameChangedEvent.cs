
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    public sealed class LoginNameChangedEvent : DomainEvent
    {
        public LoginNameChangedEvent(IAcSession acSession, AccountBase source)
            : base(acSession, source)
        {
        }
    }
}
