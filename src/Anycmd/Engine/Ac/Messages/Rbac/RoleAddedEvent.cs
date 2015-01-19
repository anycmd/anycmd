
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using Abstractions.Rbac;
    using InOuts;

    public class RoleAddedEvent : EntityAddedEvent<IRoleCreateIo>
    {
        public RoleAddedEvent(IUserSession userSession, RoleBase source, IRoleCreateIo output)
            : base(userSession, source, output)
        {
        }
    }
}
