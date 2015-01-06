
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoGroupCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveInfoGroupCommand(Guid infoGroupId)
            : base(infoGroupId)
        {

        }
    }
}
