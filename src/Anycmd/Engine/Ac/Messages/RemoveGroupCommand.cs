
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveGroupCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveGroupCommand(Guid groupId)
            : base(groupId)
        {

        }
    }
}
