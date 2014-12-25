
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveButtonCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveButtonCommand(Guid buttonId)
            : base(buttonId)
        {

        }
    }
}
