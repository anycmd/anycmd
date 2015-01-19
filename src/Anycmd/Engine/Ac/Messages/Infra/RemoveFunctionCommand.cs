
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveFunctionCommand : RemoveEntityCommand
    {
        public RemoveFunctionCommand(IUserSession userSession, Guid functionId)
            : base(userSession, functionId)
        {

        }
    }
}
