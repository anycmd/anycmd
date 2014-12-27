
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;


    public class RemovePropertyCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemovePropertyCommand(Guid propertyId)
            : base(propertyId)
        {

        }
    }
}
