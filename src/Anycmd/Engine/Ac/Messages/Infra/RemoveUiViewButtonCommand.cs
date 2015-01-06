
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveUiViewButtonCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveUiViewButtonCommand(Guid viewButtonId)
            : base(viewButtonId)
        {

        }
    }
}
