
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveNodeElementCareCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveNodeElementCareCommand(Guid nodeElementCareId)
            : base(nodeElementCareId)
        {

        }
    }
}
