
namespace Anycmd.Engine.Ac.Groups
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
