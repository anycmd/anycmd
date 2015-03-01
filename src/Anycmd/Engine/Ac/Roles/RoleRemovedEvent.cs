
namespace Anycmd.Engine.Ac.Roles
{
    using Events;

    public sealed class RoleRemovedEvent : DomainEvent
    {
        public RoleRemovedEvent(IAcSession acSession, RoleBase source)
            : base(acSession, source)
        {
        }

        internal RoleRemovedEvent(IAcSession acSession, RoleBase source, bool isPrivate)
            : this(acSession, source)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}