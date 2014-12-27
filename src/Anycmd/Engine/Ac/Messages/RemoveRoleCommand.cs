
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveRoleCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveRoleCommand(Guid roleId)
            : base(roleId)
        {

        }
    }
}
