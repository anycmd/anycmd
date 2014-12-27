
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveDicCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveDicCommand(Guid dicId)
            : base(dicId)
        {

        }
    }
}
