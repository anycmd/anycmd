
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemovePositionCommand: RemoveEntityCommand, IAnycmdCommand
    {
        public RemovePositionCommand(Guid groupId)
            : base(groupId)
        {

        }
    }
}
