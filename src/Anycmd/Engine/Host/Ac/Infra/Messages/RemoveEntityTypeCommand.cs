
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveEntityTypeCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveEntityTypeCommand(Guid entityTypeId)
            : base(entityTypeId)
        {

        }
    }
}
