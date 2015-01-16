
namespace Anycmd.Engine.Ac.Messages.Rbac
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
