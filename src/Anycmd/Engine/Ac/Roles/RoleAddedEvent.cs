
namespace Anycmd.Engine.Ac.Roles
{
    using Messages;

    public sealed class RoleAddedEvent : EntityAddedEvent<IRoleCreateIo>
    {
        public RoleAddedEvent(IAcSession acSession, RoleBase source, IRoleCreateIo output)
            : base(acSession, source, output)
        {
        }

        internal bool IsPrivate { get; set; }
    }
}
