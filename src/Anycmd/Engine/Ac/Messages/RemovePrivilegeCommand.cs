
namespace Anycmd.Engine.Ac.Messages
{
    using System;

    public class RemovePrivilegeCommand : RemoveEntityCommand
    {
        public RemovePrivilegeCommand(IUserSession userSession, Guid privilegeBigramId)
            : base(userSession, privilegeBigramId)
        {

        }
    }
}
