
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveActionCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveActionCommand(Guid actionId)
            : base(actionId)
        {

        }
    }
}
