
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemovePositionCommand: RemoveEntityCommand
    {
        public RemovePositionCommand(IAcSession userSession, Guid groupId)
            : base(userSession, groupId)
        {

        }
    }
}
