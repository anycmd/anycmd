
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveProcessCommand : RemoveEntityCommand
    {
        public RemoveProcessCommand(IAcSession userSession, Guid processId)
            : base(userSession, processId)
        {

        }
    }
}
