
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using System;

    public class EnableAccountCommand : Command
    {
        public EnableAccountCommand(IAcSession userSession, Guid accountId)
        {
            this.AcSession = userSession;
            this.AccountId = accountId;
        }

        public IAcSession AcSession { get; private set; }

        public Guid AccountId { get; private set; }
    }
}
