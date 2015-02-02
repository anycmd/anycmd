
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemovePositionCommand: RemoveEntityCommand
    {
        public RemovePositionCommand(IAcSession acSession, Guid groupId)
            : base(acSession, groupId)
        {

        }
    }
}
