
namespace Anycmd.Engine.Host.Ac.Identity.Messages
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
