
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveEntityTypeCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveEntityTypeCommand(Guid entityTypeId)
            : base(entityTypeId)
        {

        }
    }
}
