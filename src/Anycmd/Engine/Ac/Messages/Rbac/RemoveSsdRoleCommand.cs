
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveSsdRoleCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveSsdRoleCommand(Guid ssdRoleId)
            : base(ssdRoleId)
        {

        }
    }
}
