
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveGroupCommand : RemoveEntityCommand
    {
        public RemoveGroupCommand(IAcSession userSession, Guid groupId)
            : base(userSession, groupId)
        {

        }
    }
}
