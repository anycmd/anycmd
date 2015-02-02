
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveFunctionCommand : RemoveEntityCommand
    {
        public RemoveFunctionCommand(IAcSession acSession, Guid functionId)
            : base(acSession, functionId)
        {

        }
    }
}
