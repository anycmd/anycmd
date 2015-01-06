
namespace Anycmd.Engine.Ac.Messages
{
    using System;

    public class RemoveSsdSetCommand: RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveSsdSetCommand(Guid ssdSetId)
            : base(ssdSetId)
        {

        }
    }
}
