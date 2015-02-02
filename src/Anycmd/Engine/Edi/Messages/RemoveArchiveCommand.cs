
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveArchiveCommand : RemoveEntityCommand
    {
        public RemoveArchiveCommand(IAcSession acSession, Guid archiveId)
            : base(acSession, archiveId)
        {

        }
    }
}
