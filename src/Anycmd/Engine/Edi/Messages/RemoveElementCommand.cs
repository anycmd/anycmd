
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveElementCommand : RemoveEntityCommand
    {
        public RemoveElementCommand(IUserSession userSession, Guid elementId)
            : base(userSession, elementId)
        {

        }
    }
}
