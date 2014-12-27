
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveInfoGroupCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveInfoGroupCommand(Guid infoGroupId)
            : base(infoGroupId)
        {

        }
    }
}
