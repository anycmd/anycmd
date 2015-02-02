
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;


    public class RemovePropertyCommand : RemoveEntityCommand
    {
        public RemovePropertyCommand(IAcSession acSession, Guid propertyId)
            : base(acSession, propertyId)
        {

        }
    }
}
