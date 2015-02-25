
namespace Anycmd.Engine.Ac.Accounts
{
    using Events;

    public class DeveloperUpdatedEvent : DomainEvent
    {
        public DeveloperUpdatedEvent(IAcSession acSession, AccountBase source) : base(acSession, source) { }
    }
}
