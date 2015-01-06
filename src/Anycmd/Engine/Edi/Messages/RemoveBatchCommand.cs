
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveBatchCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveBatchCommand(Guid batchId)
            : base(batchId)
        {

        }
    }
}
