
namespace Anycmd.Engine.Ac.Privileges
{
    using Abstractions;
    using Events;

    public class PrivilegeRemovedEvent : DomainEvent
    {
        public PrivilegeRemovedEvent(IAcSession acSession, PrivilegeBase source)
            : base(acSession, source)
        {
        }
    }
}
