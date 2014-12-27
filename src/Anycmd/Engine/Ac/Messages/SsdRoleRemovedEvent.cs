
namespace Anycmd.Engine.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;

    public class SsdRoleRemovedEvent : DomainEvent
    {
        public SsdRoleRemovedEvent(SsdRoleBase source)
            : base(source)
        {
        }
    }
}