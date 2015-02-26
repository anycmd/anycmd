
namespace Anycmd.Engine.Ac.Ssd
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
