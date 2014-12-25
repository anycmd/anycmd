
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
