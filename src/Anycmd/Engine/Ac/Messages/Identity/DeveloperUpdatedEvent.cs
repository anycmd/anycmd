
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Abstractions.Identity;
    using Events;

    public class DeveloperUpdatedEvent : DomainEvent
    {
        public DeveloperUpdatedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}
