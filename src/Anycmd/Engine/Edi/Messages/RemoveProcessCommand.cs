
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveProcessCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveProcessCommand(Guid processId)
            : base(processId)
        {

        }
    }
}
