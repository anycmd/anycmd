
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;


    public class RemovePropertyCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemovePropertyCommand(Guid propertyId)
            : base(propertyId)
        {

        }
    }
}
