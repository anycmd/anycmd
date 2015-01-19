
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveEntityTypeCommand : RemoveEntityCommand
    {
        public RemoveEntityTypeCommand(IUserSession userSession, Guid entityTypeId)
            : base(userSession, entityTypeId)
        {

        }
    }
}
