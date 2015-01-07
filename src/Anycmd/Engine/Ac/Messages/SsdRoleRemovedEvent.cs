
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class SsdRoleRemovedEvent : DomainEvent
    {
        public SsdRoleRemovedEvent(SsdRoleBase source)
            : base(source)
        {
        }
    }
}