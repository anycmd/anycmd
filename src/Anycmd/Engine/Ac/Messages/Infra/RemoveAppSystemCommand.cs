
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using Commands;
    using Model;
    using System;

    public class RemoveAppSystemCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveAppSystemCommand(Guid appSystemId)
            : base(appSystemId)
        {

        }
    }
}
