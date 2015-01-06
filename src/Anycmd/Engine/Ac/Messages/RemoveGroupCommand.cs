
namespace Anycmd.Engine.Ac.Messages
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
