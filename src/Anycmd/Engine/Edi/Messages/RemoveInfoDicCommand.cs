
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoDicCommand : RemoveEntityCommand
    {
        public RemoveInfoDicCommand(IUserSession userSession, Guid infoDicId)
            : base(userSession, infoDicId)
        {

        }
    }
}
