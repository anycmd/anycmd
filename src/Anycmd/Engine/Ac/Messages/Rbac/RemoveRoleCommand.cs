
namespace Anycmd.Engine.Ac.Messages.Rbac
{
    using System;

    public class RemoveRoleCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveRoleCommand(Guid roleId)
            : base(roleId)
        {

        }
    }
}
