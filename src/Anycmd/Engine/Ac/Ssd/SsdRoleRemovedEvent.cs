
namespace Anycmd.Engine.Ac.Ssd
{
    using Events;

    public sealed class SsdRoleRemovedEvent : DomainEvent
    {
        public SsdRoleRemovedEvent(IAcSession acSession, SsdRoleBase source)
            : base(acSession, source)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}