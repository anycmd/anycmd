
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Engine.Ac.Abstractions.Identity;
    using Events;

    public class DeveloperUpdatedEvent : DomainEvent
    {
        public DeveloperUpdatedEvent(AccountBase source) : base(source) { }
    }
}
