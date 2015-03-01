
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveTopicCommand : RemoveEntityCommand
    {
        public RemoveTopicCommand(IAcSession acSession, Guid eventTopicId)
            : base(acSession, eventTopicId)
        {

        }
    }
}
