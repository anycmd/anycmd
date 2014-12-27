
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveTopicCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveTopicCommand(Guid eventTopicId)
            : base(eventTopicId)
        {

        }
    }
}
