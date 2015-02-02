
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeCommand : RemoveEntityCommand
    {
        public RemoveNodeCommand(IAcSession userSession, Guid nodeId)
            : base(userSession, nodeId)
        {

        }
    }
}
