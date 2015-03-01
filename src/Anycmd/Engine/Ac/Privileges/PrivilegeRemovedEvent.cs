
namespace Anycmd.Engine.Ac.Privileges
{
    using Events;

    public sealed class PrivilegeRemovedEvent : DomainEvent
    {
        public PrivilegeRemovedEvent(IAcSession acSession, PrivilegeBase source)
            : base(acSession, source)
        {
        }

        internal PrivilegeRemovedEvent(IAcSession acSession, PrivilegeBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
