
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveRoleCommand : RemoveEntityCommand
    {
        public RemoveRoleCommand(IAcSession userSession, Guid roleId)
            : base(userSession, roleId)
        {

        }
    }
}
