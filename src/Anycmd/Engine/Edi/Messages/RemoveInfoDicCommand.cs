
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveInfoDicCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveInfoDicCommand(Guid infoDicId)
            : base(infoDicId)
        {

        }
    }
}
