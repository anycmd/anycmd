
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    public class DeveloperUpdatedEvent : DomainEvent
    {
        public DeveloperUpdatedEvent(AccountBase source) : base(source) { }
    }
}
