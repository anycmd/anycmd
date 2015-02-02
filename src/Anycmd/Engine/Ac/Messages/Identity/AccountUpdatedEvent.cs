
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class AccountUpdatedEvent : DomainEvent
    {
        public AccountUpdatedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}