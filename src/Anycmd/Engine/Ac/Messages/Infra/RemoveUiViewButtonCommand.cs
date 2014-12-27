
namespace Anycmd.Engine.Ac.Messages.Infra
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
