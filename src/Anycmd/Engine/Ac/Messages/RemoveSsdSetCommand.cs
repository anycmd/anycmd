
namespace Anycmd.Engine.Ac.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveSsdSetCommand: RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveSsdSetCommand(Guid ssdSetId)
            : base(ssdSetId)
        {

        }
    }
}
