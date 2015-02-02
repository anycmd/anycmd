
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class RoleRemovingEvent: DomainEvent
    {
        public RoleRemovingEvent(IAcSession acSession, RoleBase source)
            : base(acSession, source)
        {
        }
    }
}