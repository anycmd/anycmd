
namespace Anycmd.Engine.Ac.Ssd
{
    using Events;

    public sealed class SsdRoleRemovedEvent : DomainEvent
    {
        public SsdRoleRemovedEvent(IAcSession acSession, SsdRoleBase source)
            : base(acSession, source)
        {
        }

        internal SsdRoleRemovedEvent(IAcSession acSession, SsdRoleBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}