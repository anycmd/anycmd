
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class GroupRemovedEvent : DomainEvent
    {
        public GroupRemovedEvent(IAcSession userSession, GroupBase source)
            : base(userSession, source)
        {
        }
    }
}