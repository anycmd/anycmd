
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveUiViewCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveUiViewCommand(Guid viewId)
            : base(viewId)
        {

        }
    }
}
