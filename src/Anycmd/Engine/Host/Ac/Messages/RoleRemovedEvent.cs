
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;

    public class RoleRemovedEvent : DomainEvent
    {
        public RoleRemovedEvent(RoleBase source)
            : base(source)
        {
        }
    }
}