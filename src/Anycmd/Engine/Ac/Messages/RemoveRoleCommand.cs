
namespace Anycmd.Engine.Ac.Messages
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
