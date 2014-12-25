
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
