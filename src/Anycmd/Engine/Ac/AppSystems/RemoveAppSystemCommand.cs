
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveAppSystemCommand : RemoveEntityCommand
    {
        public RemoveAppSystemCommand(IAcSession acSession, Guid appSystemId)
            : base(acSession, appSystemId)
        {

        }
    }
}
