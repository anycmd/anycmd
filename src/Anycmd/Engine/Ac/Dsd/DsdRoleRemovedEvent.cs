
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class DsdRoleRemovedEvent : DomainEvent
    {
        public DsdRoleRemovedEvent(IAcSession acSession, DsdRoleBase source)
            : base(acSession, source)
        {
        }
    }
}