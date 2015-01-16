
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class GroupRemovedEvent : DomainEvent
    {
        public GroupRemovedEvent(GroupBase source)
            : base(source)
        {
        }
    }
}