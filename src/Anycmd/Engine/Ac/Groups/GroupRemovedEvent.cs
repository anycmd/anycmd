
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Abstractions.Infra;
    using Events;

    public class GroupRemovedEvent : DomainEvent
    {
        public GroupRemovedEvent(IAcSession acSession, GroupBase source)
            : base(acSession, source)
        {
        }
    }
}