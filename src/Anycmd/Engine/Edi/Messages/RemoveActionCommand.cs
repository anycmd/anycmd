
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveActionCommand : RemoveEntityCommand
    {
        public RemoveActionCommand(IAcSession acSession, Guid actionId)
            : base(acSession, actionId)
        {

        }
    }
}
