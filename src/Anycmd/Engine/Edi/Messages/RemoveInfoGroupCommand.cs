
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoGroupCommand : RemoveEntityCommand
    {
        public RemoveInfoGroupCommand(IUserSession userSession, Guid infoGroupId)
            : base(userSession, infoGroupId)
        {

        }
    }
}
