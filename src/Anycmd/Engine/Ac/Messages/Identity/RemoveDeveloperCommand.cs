
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using System;

    public class RemoveDeveloperCommand: Command, IAnycmdCommand
    {
        public RemoveDeveloperCommand(IUserSession userSession, Guid accountId)
        {
            this.UserSession = userSession;
            this.AccountId = accountId;
        }

        public IUserSession UserSession { get; private set; }

        public Guid AccountId { get; private set; }
    }
}
