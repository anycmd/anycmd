
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveButtonCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveButtonCommand(Guid buttonId)
            : base(buttonId)
        {

        }
    }
}
