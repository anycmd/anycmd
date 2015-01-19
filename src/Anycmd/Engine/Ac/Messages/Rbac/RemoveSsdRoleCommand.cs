
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveSsdRoleCommand : RemoveEntityCommand
    {
        public RemoveSsdRoleCommand(IUserSession userSession, Guid ssdRoleId)
            : base(userSession, ssdRoleId)
        {

        }
    }
}
