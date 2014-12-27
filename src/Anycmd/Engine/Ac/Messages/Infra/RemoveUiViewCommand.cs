
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveUiViewCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveUiViewCommand(Guid viewId)
            : base(viewId)
        {

        }
    }
}
