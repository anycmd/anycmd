
namespace Anycmd.Engine.Ac.Dsd
{
    using Events;

    public sealed class DsdRoleRemovedEvent : DomainEvent
    {
        public DsdRoleRemovedEvent(IAcSession acSession, DsdRoleBase source)
            : base(acSession, source)
        {
        }

        internal DsdRoleRemovedEvent(IAcSession acSession, DsdRoleBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}