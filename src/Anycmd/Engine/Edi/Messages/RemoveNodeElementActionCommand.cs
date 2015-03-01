
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveNodeElementActionCommand : RemoveEntityCommand
    {
        public RemoveNodeElementActionCommand(IAcSession acSession, Guid nodeElementActionId)
            : base(acSession, nodeElementActionId)
        {

        }
    }
}
