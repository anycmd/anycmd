
namespace Anycmd.Engine.Ac.Roles
{
    using Events;

    public sealed class RoleRemovedEvent : DomainEvent
    {
        public RoleRemovedEvent(IAcSession acSession, RoleBase source)
            : base(acSession, source)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}