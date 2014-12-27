
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveDicCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveDicCommand(Guid dicId)
            : base(dicId)
        {

        }
    }
}
