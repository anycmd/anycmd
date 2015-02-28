
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public class RemoveInfoDicItemCommand : RemoveEntityCommand
    {
        public RemoveInfoDicItemCommand(IAcSession acSession, Guid infoDicItemId)
            : base(acSession, infoDicItemId)
        {

        }
    }
}
