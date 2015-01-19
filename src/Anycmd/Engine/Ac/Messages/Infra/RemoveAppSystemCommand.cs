
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveAppSystemCommand : RemoveEntityCommand
    {
        public RemoveAppSystemCommand(IUserSession userSession, Guid appSystemId)
            : base(userSession, appSystemId)
        {

        }
    }
}
