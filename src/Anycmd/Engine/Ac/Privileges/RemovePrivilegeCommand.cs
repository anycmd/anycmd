
namespace Anycmd.Engine.Ac.Privileges
{
    using Messages;
    using System;

    public sealed class RemovePrivilegeCommand : RemoveEntityCommand
    {
        public RemovePrivilegeCommand(IAcSession acSession, Guid privilegeBigramId)
            : base(acSession, privilegeBigramId)
        {

        }
    }
}
