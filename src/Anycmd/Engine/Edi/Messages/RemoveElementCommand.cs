
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveElementCommand : RemoveEntityCommand
    {
        public RemoveElementCommand(IAcSession userSession, Guid elementId)
            : base(userSession, elementId)
        {

        }
    }
}
