
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public class RemoveActionCommand : RemoveEntityCommand
    {
        public RemoveActionCommand(IAcSession acSession, Guid actionId)
            : base(acSession, actionId)
        {

        }
    }
}
