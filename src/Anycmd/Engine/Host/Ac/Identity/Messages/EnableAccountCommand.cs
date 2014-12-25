
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Commands;
    using System;

    public class EnableAccountCommand : Command
    {
        public EnableAccountCommand(Guid accountId)
        {
            this.AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}
