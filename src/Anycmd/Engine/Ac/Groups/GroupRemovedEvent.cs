
namespace Anycmd.Engine.Ac.Groups
{
    using Groups;
    using Events;

    public class GroupRemovedEvent : DomainEvent
    {
        public GroupRemovedEvent(IAcSession acSession, GroupBase source)
            : base(acSession, source)
        {
        }
    }
}