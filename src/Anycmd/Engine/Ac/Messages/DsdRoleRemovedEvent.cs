
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class DsdRoleRemovedEvent : DomainEvent
    {
        public DsdRoleRemovedEvent(DsdRoleBase source)
            : base(source)
        {
        }
    }
}