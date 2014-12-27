
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveAppSystemCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveAppSystemCommand(Guid appSystemId)
            : base(appSystemId)
        {

        }
    }
}
