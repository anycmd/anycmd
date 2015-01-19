
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveDsdRoleCommand : RemoveEntityCommand
    {
        public RemoveDsdRoleCommand(IUserSession userSession, Guid dsdRoleId)
            : base(userSession, dsdRoleId)
        {

        }
    }
}
