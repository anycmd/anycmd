
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeCommand : RemoveEntityCommand
    {
        public RemoveNodeCommand(IUserSession userSession, Guid nodeId)
            : base(userSession, nodeId)
        {

        }
    }
}
