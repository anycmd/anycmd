
namespace Anycmd.Engine.Ac.Messages
{
    using Abstractions;
    using Events;

    public class GroupRemovingEvent: DomainEvent
    {
        public GroupRemovingEvent(GroupBase source)
            : base(source)
        {
        }
    }
}