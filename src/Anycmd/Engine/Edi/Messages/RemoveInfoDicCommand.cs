
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoDicCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveInfoDicCommand(Guid infoDicId)
            : base(infoDicId)
        {

        }
    }
}
