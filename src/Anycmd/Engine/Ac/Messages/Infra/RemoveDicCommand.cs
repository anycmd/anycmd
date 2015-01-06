
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveDicCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveDicCommand(Guid dicId)
            : base(dicId)
        {

        }
    }
}
