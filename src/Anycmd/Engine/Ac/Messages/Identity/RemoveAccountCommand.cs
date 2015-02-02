
namespace Anycmd.Engine.Ac.Messages.Identity
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
