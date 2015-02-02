
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveTopicCommand : RemoveEntityCommand
    {
        public RemoveTopicCommand(IAcSession acSession, Guid eventTopicId)
            : base(acSession, eventTopicId)
        {

        }
    }
}
