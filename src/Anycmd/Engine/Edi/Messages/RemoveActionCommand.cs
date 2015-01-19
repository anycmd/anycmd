
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveActionCommand : RemoveEntityCommand
    {
        public RemoveActionCommand(IUserSession userSession, Guid actionId)
            : base(userSession, actionId)
        {

        }
    }
}
