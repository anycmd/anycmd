
namespace Anycmd.Engine.Ac.Ssd
{
    using Events;

    public class SsdRoleRemovedEvent : DomainEvent
    {
        public SsdRoleRemovedEvent(IAcSession acSession, SsdRoleBase source)
            : base(acSession, source)
        {
        }
    }
}