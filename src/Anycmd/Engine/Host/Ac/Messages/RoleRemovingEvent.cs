
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;

    public class RoleRemovingEvent: DomainEvent
    {
        public RoleRemovingEvent(RoleBase source)
            : base(source)
        {
        }
    }
}