
namespace Anycmd.Engine.Ac.Dsd
{
    using Events;

    public sealed class DsdRoleRemovedEvent : DomainEvent
    {
        public DsdRoleRemovedEvent(IAcSession acSession, DsdRoleBase source)
            : base(acSession, source)
        {
        }
        internal bool IsPrivate { get; set; }
    }
}