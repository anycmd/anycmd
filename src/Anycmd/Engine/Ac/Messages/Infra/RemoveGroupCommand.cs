
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveGroupCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveGroupCommand(Guid groupId)
            : base(groupId)
        {

        }
    }
}
