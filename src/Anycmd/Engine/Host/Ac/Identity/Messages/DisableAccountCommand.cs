
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Commands;
    using System;

    public class DisableAccountCommand : Command, ISysCommand
    {
        public DisableAccountCommand(Guid accountId)
        {
            this.AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}
