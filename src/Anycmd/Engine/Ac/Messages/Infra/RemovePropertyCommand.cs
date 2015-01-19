
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;


    public class RemovePropertyCommand : RemoveEntityCommand
    {
        public RemovePropertyCommand(IUserSession userSession, Guid propertyId)
            : base(userSession, propertyId)
        {

        }
    }
}
