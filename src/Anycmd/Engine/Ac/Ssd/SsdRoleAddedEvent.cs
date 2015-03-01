
namespace Anycmd.Engine.Ac.Ssd
{
    using Messages;

    public sealed class SsdRoleAddedEvent : EntityAddedEvent<ISsdRoleCreateIo>
    {
        public SsdRoleAddedEvent(IAcSession acSession, SsdRoleBase source, ISsdRoleCreateIo output)
            : base(acSession, source, output)
        {
        }

        internal SsdRoleAddedEvent(IAcSession acSession, SsdRoleBase source, ISsdRoleCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
