
namespace Anycmd.Engine.Ac.Roles
{
    using Roles;
    using Events;

    public class RoleRemovedEvent : DomainEvent
    {
        public RoleRemovedEvent(IAcSession acSession, RoleBase source)
            : base(acSession, source)
        {
        }
    }
}