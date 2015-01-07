
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class GroupRemovedEvent : DomainEvent
    {
        public GroupRemovedEvent(GroupBase source)
            : base(source)
        {
        }
    }
}