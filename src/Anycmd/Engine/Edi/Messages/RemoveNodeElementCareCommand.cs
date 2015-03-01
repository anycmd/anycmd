
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveNodeElementCareCommand : RemoveEntityCommand
    {
        public RemoveNodeElementCareCommand(IAcSession acSession, Guid nodeElementCareId)
            : base(acSession, nodeElementCareId)
        {

        }
    }
}
