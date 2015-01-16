
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveDsdRoleCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveDsdRoleCommand(Guid dsdRoleId)
            : base(dsdRoleId)
        {

        }
    }
}
