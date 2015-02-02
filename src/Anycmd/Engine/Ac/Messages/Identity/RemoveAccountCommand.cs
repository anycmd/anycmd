
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using System;

    public class RemoveAccountCommand : RemoveEntityCommand
    {
        public RemoveAccountCommand(IAcSession userSession, Guid accountId)
            : base(userSession, accountId)
        {

        }
    }
}
