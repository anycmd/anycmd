
namespace Anycmd.Engine.Ac.Roles
{
    using Messages;

    public sealed class RoleAddedEvent : EntityAddedEvent<IRoleCreateIo>
    {
        public RoleAddedEvent(IAcSession acSession, RoleBase source, IRoleCreateIo output)
            : base(acSession, source, output)
        {
        }

        internal RoleAddedEvent(IAcSession acSession, RoleBase source, IRoleCreateIo input, bool isPrivate)
            : this(acSession, source, input)
        {
            this.IsPrivate = isPrivate;
        }

        internal bool IsPrivate { get; private set; }
    }
}
