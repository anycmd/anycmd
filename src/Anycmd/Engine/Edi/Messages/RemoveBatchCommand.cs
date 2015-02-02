
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveBatchCommand : RemoveEntityCommand
    {
        public RemoveBatchCommand(IAcSession userSession, Guid batchId)
            : base(userSession, batchId)
        {

        }
    }
}
