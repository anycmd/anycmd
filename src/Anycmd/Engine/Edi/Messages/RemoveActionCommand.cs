
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveActionCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveActionCommand(Guid actionId)
            : base(actionId)
        {

        }
    }
}
