
namespace Anycmd.Engine.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveTopicCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveTopicCommand(Guid eventTopicId)
            : base(eventTopicId)
        {

        }
    }
}
