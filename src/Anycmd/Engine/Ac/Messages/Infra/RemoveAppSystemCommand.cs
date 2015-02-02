
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveAppSystemCommand : RemoveEntityCommand
    {
        public RemoveAppSystemCommand(IAcSession userSession, Guid appSystemId)
            : base(userSession, appSystemId)
        {

        }
    }
}
