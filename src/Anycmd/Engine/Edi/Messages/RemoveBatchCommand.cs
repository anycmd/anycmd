
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveBatchCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveBatchCommand(Guid batchId)
            : base(batchId)
        {

        }
    }
}
