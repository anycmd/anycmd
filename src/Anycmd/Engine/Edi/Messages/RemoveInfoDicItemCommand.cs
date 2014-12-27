
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveInfoDicItemCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveInfoDicItemCommand(Guid infoDicItemId)
            : base(infoDicItemId)
        {

        }
    }
}
