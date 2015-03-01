
namespace Anycmd.Engine.Ac.Privileges
{
    using Events;

    public sealed class PrivilegeRemovedEvent : DomainEvent
    {
        public PrivilegeRemovedEvent(IAcSession acSession, PrivilegeBase source)
            : base(acSession, source)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
