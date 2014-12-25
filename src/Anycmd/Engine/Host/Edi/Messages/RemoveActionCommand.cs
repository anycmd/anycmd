
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveActionCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveActionCommand(Guid actionId)
            : base(actionId)
        {

        }
    }
}
