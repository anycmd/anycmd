
namespace Anycmd.Engine.Ac.Messages.Infra
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
