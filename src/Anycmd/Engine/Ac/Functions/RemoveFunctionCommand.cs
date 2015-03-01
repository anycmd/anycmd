
namespace Anycmd.Engine.Ac.Functions
{
    using Messages;
    using System;

    public sealed class RemoveFunctionCommand : RemoveEntityCommand
    {
        public RemoveFunctionCommand(IAcSession acSession, Guid functionId)
            : base(acSession, functionId)
        {

        }
    }
}
