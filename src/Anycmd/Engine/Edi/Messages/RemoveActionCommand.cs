
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveActionCommand : RemoveEntityCommand
    {
        public RemoveActionCommand(IAcSession userSession, Guid actionId)
            : base(userSession, actionId)
        {

        }
    }
}
