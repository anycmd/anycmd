
namespace Anycmd.Engine.Ac.Groups
{
    using Events;

    public sealed class GroupRemovedEvent : DomainEvent
    {
        public GroupRemovedEvent(IAcSession acSession, GroupBase source)
            : base(acSession, source)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}