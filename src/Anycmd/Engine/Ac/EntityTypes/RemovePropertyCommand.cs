
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;
    using System;


    public sealed class RemovePropertyCommand : RemoveEntityCommand
    {
        public RemovePropertyCommand(IAcSession acSession, Guid propertyId)
            : base(acSession, propertyId)
        {

        }
    }
}
