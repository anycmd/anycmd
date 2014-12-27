
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using System;

    public class RemoveDeveloperCommand: Command, IAnycmdCommand
    {
        public RemoveDeveloperCommand(Guid accountId)
        {
            this.AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}
