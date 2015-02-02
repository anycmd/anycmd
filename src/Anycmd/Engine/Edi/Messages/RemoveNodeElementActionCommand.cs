
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeElementActionCommand : RemoveEntityCommand
    {
        public RemoveNodeElementActionCommand(IAcSession userSession, Guid nodeElementActionId)
            : base(userSession, nodeElementActionId)
        {

        }
    }
}
