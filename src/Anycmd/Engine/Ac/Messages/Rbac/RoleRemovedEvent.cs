
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class RoleRemovedEvent : DomainEvent
    {
        public RoleRemovedEvent(IAcSession acSession, RoleBase source)
            : base(acSession, source)
        {
        }
    }
}