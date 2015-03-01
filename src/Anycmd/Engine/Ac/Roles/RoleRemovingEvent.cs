
namespace Anycmd.Engine.Ac.Roles
{
    using Events;

    public sealed class RoleRemovingEvent : DomainEvent
    {
        public RoleRemovingEvent(IAcSession acSession, RoleBase source)
            : base(acSession, source)
        {
        }
    }
}