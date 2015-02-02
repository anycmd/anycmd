
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveGroupCommand : RemoveEntityCommand
    {
        public RemoveGroupCommand(IAcSession acSession, Guid groupId)
            : base(acSession, groupId)
        {

        }
    }
}
