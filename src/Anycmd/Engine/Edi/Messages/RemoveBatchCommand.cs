
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveBatchCommand : RemoveEntityCommand
    {
        public RemoveBatchCommand(IUserSession userSession, Guid batchId)
            : base(userSession, batchId)
        {

        }
    }
}
