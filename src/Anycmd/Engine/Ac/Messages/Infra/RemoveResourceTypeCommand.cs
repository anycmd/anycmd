
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveResourceTypeCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveResourceTypeCommand(Guid resourceTypeId)
            : base(resourceTypeId)
        {

        }
    }
}
