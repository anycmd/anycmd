
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoDicItemCommand : RemoveEntityCommand
    {
        public RemoveInfoDicItemCommand(IAcSession userSession, Guid infoDicItemId)
            : base(userSession, infoDicItemId)
        {

        }
    }
}
