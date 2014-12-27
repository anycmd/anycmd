
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveNodeCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveNodeCommand(Guid nodeId)
            : base(nodeId)
        {

        }
    }
}
