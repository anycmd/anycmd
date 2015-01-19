
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using System;

    public class DisableAccountCommand : Command, IAnycmdCommand
    {
        public DisableAccountCommand(IUserSession userSession, Guid accountId)
        {
            this.UserSession = userSession;
            this.AccountId = accountId;
        }

        public IUserSession UserSession { get; private set; }

        public Guid AccountId { get; private set; }
    }
}
