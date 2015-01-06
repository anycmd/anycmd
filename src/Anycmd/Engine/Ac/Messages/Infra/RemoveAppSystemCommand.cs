
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveAppSystemCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveAppSystemCommand(Guid appSystemId)
            : base(appSystemId)
        {

        }
    }
}
