
namespace Anycmd.Engine.Ac.EntityTypes
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
