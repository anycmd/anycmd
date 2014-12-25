
namespace Anycmd.Engine.Host.Ac.Infra.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveUiViewButtonCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveUiViewButtonCommand(Guid viewButtonId)
            : base(viewButtonId)
        {

        }
    }
}
