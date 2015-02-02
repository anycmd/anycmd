
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using System;

    public class EnableAccountCommand : Command
    {
        public EnableAccountCommand(IAcSession acSession, Guid accountId)
        {
            this.AcSession = acSession;
            this.AccountId = accountId;
        }

        public IAcSession AcSession { get; private set; }

        public Guid AccountId { get; private set; }
    }
}
