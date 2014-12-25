
namespace Anycmd.Engine.Host.Ac.Infra.Messages
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
