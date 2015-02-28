
namespace Anycmd.Engine.Ac.Privileges
{
    using Messages;
    using System;

    public class RemovePrivilegeCommand : RemoveEntityCommand
    {
        public RemovePrivilegeCommand(IAcSession acSession, Guid privilegeBigramId)
            : base(acSession, privilegeBigramId)
        {

        }
    }
}
