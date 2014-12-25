
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Commands;
    using System;

    public class RemoveDeveloperCommand: Command, ISysCommand
    {
        public RemoveDeveloperCommand(Guid accountId)
        {
            this.AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}
