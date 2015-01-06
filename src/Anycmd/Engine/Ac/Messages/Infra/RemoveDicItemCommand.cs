
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveDicItemCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveDicItemCommand(Guid dicItemId)
            : base(dicItemId)
        {

        }
    }
}
