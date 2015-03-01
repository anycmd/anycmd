
namespace Anycmd.Engine.Edi.Messages
{
    using Engine.Messages;
    using System;

    public sealed class RemoveArchiveCommand : RemoveEntityCommand
    {
        public RemoveArchiveCommand(IAcSession acSession, Guid archiveId)
            : base(acSession, archiveId)
        {

        }
    }
}
