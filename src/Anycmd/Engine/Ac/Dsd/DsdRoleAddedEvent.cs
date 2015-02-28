
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;

    public sealed class DsdRoleAddedEvent : EntityAddedEvent<IDsdRoleCreateIo>
    {
        public DsdRoleAddedEvent(IAcSession acSession, DsdRoleBase source, IDsdRoleCreateIo output)
            : base(acSession, source, output)
        {
        }
        internal bool IsPrivate { get; set; }
    }
}
