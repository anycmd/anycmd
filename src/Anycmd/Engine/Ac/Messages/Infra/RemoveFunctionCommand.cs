
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveFunctionCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveFunctionCommand(Guid functionId)
            : base(functionId)
        {

        }
    }
}
