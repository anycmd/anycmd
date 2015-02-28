
namespace Anycmd.Engine.Ac.Roles
{
    using Messages;
    using System;

    public class RemoveRoleCommand : RemoveEntityCommand
    {
        public RemoveRoleCommand(IAcSession acSession, Guid roleId)
            : base(acSession, roleId)
        {

        }
    }
}
