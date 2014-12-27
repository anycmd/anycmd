
namespace Anycmd.Engine.Ac.Messages.Identity
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
