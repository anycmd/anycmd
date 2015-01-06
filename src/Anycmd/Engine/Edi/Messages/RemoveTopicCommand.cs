
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveTopicCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveTopicCommand(Guid eventTopicId)
            : base(eventTopicId)
        {

        }
    }
}
