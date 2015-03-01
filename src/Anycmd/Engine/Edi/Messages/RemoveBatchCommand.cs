
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveBatchCommand : RemoveEntityCommand
    {
        public RemoveBatchCommand(IAcSession acSession, Guid batchId)
            : base(acSession, batchId)
        {

        }
    }
}
