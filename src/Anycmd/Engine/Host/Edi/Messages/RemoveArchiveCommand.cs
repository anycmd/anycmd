
namespace Anycmd.Engine.Host.Edi.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveArchiveCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveArchiveCommand(Guid archiveId)
            : base(archiveId)
        {

        }
    }
}
