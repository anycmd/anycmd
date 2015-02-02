
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class AccountAddedEvent : DomainEvent
    {
        public AccountAddedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}