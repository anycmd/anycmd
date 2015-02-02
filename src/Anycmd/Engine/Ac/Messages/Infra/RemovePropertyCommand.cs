
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;


    public class RemovePropertyCommand : RemoveEntityCommand
    {
        public RemovePropertyCommand(IAcSession userSession, Guid propertyId)
            : base(userSession, propertyId)
        {

        }
    }
}
