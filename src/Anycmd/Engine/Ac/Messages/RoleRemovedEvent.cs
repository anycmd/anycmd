
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class RoleRemovedEvent : DomainEvent
    {
        public RoleRemovedEvent(RoleBase source)
            : base(source)
        {
        }
    }
}