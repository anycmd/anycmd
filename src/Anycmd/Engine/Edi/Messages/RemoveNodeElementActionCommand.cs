
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeElementActionCommand : RemoveEntityCommand
    {
        public RemoveNodeElementActionCommand(IAcSession acSession, Guid nodeElementActionId)
            : base(acSession, nodeElementActionId)
        {

        }
    }
}
