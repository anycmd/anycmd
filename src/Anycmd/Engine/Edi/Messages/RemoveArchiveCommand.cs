
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveArchiveCommand : RemoveEntityCommand
    {
        public RemoveArchiveCommand(IAcSession userSession, Guid archiveId)
            : base(userSession, archiveId)
        {

        }
    }
}
