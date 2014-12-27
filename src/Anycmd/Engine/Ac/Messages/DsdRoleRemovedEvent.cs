
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;

    public class DsdRoleRemovedEvent : DomainEvent
    {
        public DsdRoleRemovedEvent(DsdRoleBase source)
            : base(source)
        {
        }
    }
}