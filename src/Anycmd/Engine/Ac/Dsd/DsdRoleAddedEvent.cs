
namespace Anycmd.Engine.Ac.Dsd
{
    using Messages;

    public sealed class DsdRoleAddedEvent : EntityAddedEvent<IDsdRoleCreateIo>
    {
        public DsdRoleAddedEvent(IAcSession acSession, DsdRoleBase source, IDsdRoleCreateIo output)
            : base(acSession, source, output)
        {
        }

        internal DsdRoleAddedEvent(IAcSession acSession, DsdRoleBase source, IDsdRoleCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
