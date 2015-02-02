
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class LoginNameChangedEvent : DomainEvent
    {
        public LoginNameChangedEvent(IAcSession userSession, AccountBase source)
            : base(userSession, source)
        {
        }
    }
}
