
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemovePositionCommand: RemoveEntityCommand, IAnycmdCommand
    {
        public RemovePositionCommand(Guid groupId)
            : base(groupId)
        {

        }
    }
}
