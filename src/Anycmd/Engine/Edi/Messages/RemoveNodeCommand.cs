
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveNodeCommand : RemoveEntityCommand
    {
        public RemoveNodeCommand(IAcSession acSession, Guid nodeId)
            : base(acSession, nodeId)
        {

        }
    }
}
