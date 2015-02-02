
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class LoginNameChangedEvent : DomainEvent
    {
        public LoginNameChangedEvent(IAcSession acSession, AccountBase source)
            : base(acSession, source)
        {
        }
    }
}
