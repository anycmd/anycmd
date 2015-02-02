
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoDicCommand : RemoveEntityCommand
    {
        public RemoveInfoDicCommand(IAcSession userSession, Guid infoDicId)
            : base(userSession, infoDicId)
        {

        }
    }
}
