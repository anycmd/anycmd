
namespace Anycmd.Engine.Ac.Groups
{
    using Events;

    public sealed class GroupRemovedEvent : DomainEvent
    {
        public GroupRemovedEvent(IAcSession acSession, GroupBase source)
            : base(acSession, source)
        {
        }

        internal GroupRemovedEvent(IAcSession acSession, GroupBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}