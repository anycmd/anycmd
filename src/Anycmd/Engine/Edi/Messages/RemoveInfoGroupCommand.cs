
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveInfoGroupCommand : RemoveEntityCommand
    {
        public RemoveInfoGroupCommand(IAcSession userSession, Guid infoGroupId)
            : base(userSession, infoGroupId)
        {

        }
    }
}
