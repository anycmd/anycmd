
namespace Anycmd.Engine.Ac.Accounts
{
    using System;

    public class RemoveAccountCommand : RemoveEntityCommand
    {
        public RemoveAccountCommand(IAcSession acSession, Guid accountId)
            : base(acSession, accountId)
        {

        }
    }
}
