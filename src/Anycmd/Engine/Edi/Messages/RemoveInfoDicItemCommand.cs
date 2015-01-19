
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoDicItemCommand : RemoveEntityCommand
    {
        public RemoveInfoDicItemCommand(IUserSession userSession, Guid infoDicItemId)
            : base(userSession, infoDicItemId)
        {

        }
    }
}
