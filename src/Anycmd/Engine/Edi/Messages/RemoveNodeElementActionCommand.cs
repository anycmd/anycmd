
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeElementActionCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveNodeElementActionCommand(Guid nodeElementActionId)
            : base(nodeElementActionId)
        {

        }
    }
}
