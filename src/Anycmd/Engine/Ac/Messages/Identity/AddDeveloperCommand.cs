
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Anycmd.Commands;
    using System;

    public class AddDeveloperCommand : Command, IAnycmdCommand
    {
        public AddDeveloperCommand(IUserSession userSession, Guid accountId)
        {
            this.UserSession = userSession;
            this.AccountId = accountId;
        }

        public IUserSession UserSession { get; private set; }
        public Guid AccountId { get; private set; }
    }
}
