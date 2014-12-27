
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    public class LoginNameChangedEvent : DomainEvent
    {
        public LoginNameChangedEvent(AccountBase source)
            : base(source)
        {
        }
    }
}
