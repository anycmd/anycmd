
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using InOuts;

    public class RoleAddedEvent : EntityAddedEvent<IRoleCreateIo>
    {
        public RoleAddedEvent(IAcSession acSession, RoleBase source, IRoleCreateIo output)
            : base(acSession, source, output)
        {
        }
    }
}
