
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using Model;
    using System;


    public class RemovePropertyCommand : RemoveEntityCommand, ISysCommand
    {
        public RemovePropertyCommand(Guid propertyId)
            : base(propertyId)
        {

        }
    }
}
