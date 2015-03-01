
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveInfoGroupCommand : RemoveEntityCommand
    {
        public RemoveInfoGroupCommand(IAcSession acSession, Guid infoGroupId)
            : base(acSession, infoGroupId)
        {

        }
    }
}
