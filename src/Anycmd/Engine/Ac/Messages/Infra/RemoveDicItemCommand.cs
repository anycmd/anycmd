
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveDicItemCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveDicItemCommand(Guid dicItemId)
            : base(dicItemId)
        {

        }
    }
}
