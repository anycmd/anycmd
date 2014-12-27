
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using System;

    public class DisableAccountCommand : Command, IAnycmdCommand
    {
        public DisableAccountCommand(Guid accountId)
        {
            this.AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}
