
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveFunctionCommand : RemoveEntityCommand
    {
        public RemoveFunctionCommand(IAcSession userSession, Guid functionId)
            : base(userSession, functionId)
        {

        }
    }
}
