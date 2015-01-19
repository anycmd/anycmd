
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveRoleCommand : RemoveEntityCommand
    {
        public RemoveRoleCommand(IUserSession userSession, Guid roleId)
            : base(userSession, roleId)
        {

        }
    }
}
