
namespace Anycmd.Engine.Ac.Roles
{
    using Messages;
    using System;

    public sealed class RemoveRoleCommand : RemoveEntityCommand
    {
        public RemoveRoleCommand(IAcSession acSession, Guid roleId)
            : base(acSession, roleId)
        {

        }
    }
}
