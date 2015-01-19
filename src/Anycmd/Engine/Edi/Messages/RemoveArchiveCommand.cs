
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveArchiveCommand : RemoveEntityCommand
    {
        public RemoveArchiveCommand(IUserSession userSession, Guid archiveId)
            : base(userSession, archiveId)
        {

        }
    }
}
