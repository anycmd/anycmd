
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemovePositionCommand: RemoveEntityCommand
    {
        public RemovePositionCommand(IUserSession userSession, Guid groupId)
            : base(userSession, groupId)
        {

        }
    }
}
