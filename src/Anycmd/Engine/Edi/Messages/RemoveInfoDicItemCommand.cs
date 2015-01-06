
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoDicItemCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveInfoDicItemCommand(Guid infoDicItemId)
            : base(infoDicItemId)
        {

        }
    }
}
