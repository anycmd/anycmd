
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveBatchCommand : RemoveEntityCommand
    {
        public RemoveBatchCommand(IAcSession acSession, Guid batchId)
            : base(acSession, batchId)
        {

        }
    }
}
