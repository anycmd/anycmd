
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveResourceTypeCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveResourceTypeCommand(Guid resourceTypeId)
            : base(resourceTypeId)
        {

        }
    }
}
