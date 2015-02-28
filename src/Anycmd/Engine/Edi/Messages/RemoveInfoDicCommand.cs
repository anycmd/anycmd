
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public class RemoveInfoDicCommand : RemoveEntityCommand
    {
        public RemoveInfoDicCommand(IAcSession acSession, Guid infoDicId)
            : base(acSession, infoDicId)
        {

        }
    }
}
