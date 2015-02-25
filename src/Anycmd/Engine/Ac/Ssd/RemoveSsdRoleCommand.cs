
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveSsdRoleCommand : RemoveEntityCommand
    {
        public RemoveSsdRoleCommand(IAcSession acSession, Guid ssdRoleId)
            : base(acSession, ssdRoleId)
        {

        }
    }
}
