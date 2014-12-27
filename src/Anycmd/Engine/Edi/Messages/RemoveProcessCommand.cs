
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveProcessCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveProcessCommand(Guid processId)
            : base(processId)
        {

        }
    }
}
