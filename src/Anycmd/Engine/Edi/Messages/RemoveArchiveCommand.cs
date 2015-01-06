
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveArchiveCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveArchiveCommand(Guid archiveId)
            : base(archiveId)
        {

        }
    }
}
