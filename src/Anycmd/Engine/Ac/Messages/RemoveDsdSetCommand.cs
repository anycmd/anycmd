
namespace Anycmd.Engine.Ac.Messages
{
    using System;

    public class RemoveDsdSetCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveDsdSetCommand(Guid dsdSetId)
            : base(dsdSetId)
        {

        }
    }
}
