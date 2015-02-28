
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public class RemoveElementCommand : RemoveEntityCommand
    {
        public RemoveElementCommand(IAcSession acSession, Guid elementId)
            : base(acSession, elementId)
        {

        }
    }
}
