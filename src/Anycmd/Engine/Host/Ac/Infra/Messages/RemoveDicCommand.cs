
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
