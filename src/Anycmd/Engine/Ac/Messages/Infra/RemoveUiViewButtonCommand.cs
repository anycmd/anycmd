
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveUiViewButtonCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveUiViewButtonCommand(Guid viewButtonId)
            : base(viewButtonId)
        {

        }
    }
}
