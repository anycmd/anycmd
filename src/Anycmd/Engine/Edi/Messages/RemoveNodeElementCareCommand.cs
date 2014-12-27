
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveNodeElementCareCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveNodeElementCareCommand(Guid nodeElementCareId)
            : base(nodeElementCareId)
        {

        }
    }
}
