
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeElementCareCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveNodeElementCareCommand(Guid nodeElementCareId)
            : base(nodeElementCareId)
        {

        }
    }
}
