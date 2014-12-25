
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveInfoDicItemCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveInfoDicItemCommand(Guid infoDicItemId)
            : base(infoDicItemId)
        {

        }
    }
}
