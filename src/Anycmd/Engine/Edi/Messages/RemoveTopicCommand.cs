
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveTopicCommand : RemoveEntityCommand
    {
        public RemoveTopicCommand(IUserSession userSession, Guid eventTopicId)
            : base(userSession, eventTopicId)
        {

        }
    }
}
