
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
