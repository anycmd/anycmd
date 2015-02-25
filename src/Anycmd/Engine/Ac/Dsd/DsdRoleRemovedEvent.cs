
namespace Anycmd.Engine.Ac.Dsd
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