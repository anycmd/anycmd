
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeElementCareCommand : RemoveEntityCommand
    {
        public RemoveNodeElementCareCommand(IAcSession userSession, Guid nodeElementCareId)
            : base(userSession, nodeElementCareId)
        {

        }
    }
}
