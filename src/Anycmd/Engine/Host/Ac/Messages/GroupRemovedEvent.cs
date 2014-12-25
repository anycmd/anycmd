
namespace Anycmd.Engine.Host.Ac.Messages
{
    using Engine.Ac.Abstractions;
    using Events;

    public class GroupRemovedEvent : DomainEvent
    {
        public GroupRemovedEvent(GroupBase source)
            : base(source)
        {
        }
    }
}