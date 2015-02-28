
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    public sealed class DeveloperUpdatedEvent : DomainEvent
    {
        public DeveloperUpdatedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}
