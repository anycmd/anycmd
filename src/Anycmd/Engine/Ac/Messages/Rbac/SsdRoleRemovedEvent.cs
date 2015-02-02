
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using Events;

    public class SsdRoleRemovedEvent : DomainEvent
    {
        public SsdRoleRemovedEvent(IAcSession acSession, SsdRoleBase source)
            : base(acSession, source)
        {
        }
    }
}