
namespace Anycmd.Engine.Ac.Groups
{
    using Messages;
    using System;

    public sealed class RemoveGroupCommand : RemoveEntityCommand
    {
        public RemoveGroupCommand(IAcSession acSession, Guid groupId)
            : base(acSession, groupId)
        {

        }
    }
}
