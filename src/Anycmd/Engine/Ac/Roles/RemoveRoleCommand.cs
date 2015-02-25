
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveRoleCommand : RemoveEntityCommand
    {
        public RemoveRoleCommand(IAcSession acSession, Guid roleId)
            : base(acSession, roleId)
        {

        }
    }
}
