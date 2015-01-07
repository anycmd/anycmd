
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class RoleRemovingEvent: DomainEvent
    {
        public RoleRemovingEvent(RoleBase source)
            : base(source)
        {
        }
    }
}