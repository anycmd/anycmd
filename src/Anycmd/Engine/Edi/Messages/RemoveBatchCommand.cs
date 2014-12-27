
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveBatchCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveBatchCommand(Guid batchId)
            : base(batchId)
        {

        }
    }
}
