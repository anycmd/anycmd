
namespace Anycmd.Engine.Ac.Messages.Rbac
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
