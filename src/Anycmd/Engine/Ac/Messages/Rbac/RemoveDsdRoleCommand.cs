
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveDsdRoleCommand : RemoveEntityCommand
    {
        public RemoveDsdRoleCommand(IAcSession userSession, Guid dsdRoleId)
            : base(userSession, dsdRoleId)
        {

        }
    }
}
