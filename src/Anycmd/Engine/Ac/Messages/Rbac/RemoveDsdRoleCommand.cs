
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveDsdRoleCommand : RemoveEntityCommand
    {
        public RemoveDsdRoleCommand(IAcSession acSession, Guid dsdRoleId)
            : base(acSession, dsdRoleId)
        {

        }
    }
}
