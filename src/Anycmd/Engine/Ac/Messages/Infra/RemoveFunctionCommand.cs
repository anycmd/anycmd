
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveFunctionCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveFunctionCommand(Guid functionId)
            : base(functionId)
        {

        }
    }
}
