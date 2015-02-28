
namespace Anycmd.Engine.Ac.Accounts
{
    using Commands;
    using Messages;
    using System;

    public sealed class DisableAccountCommand : Command, IAnycmdCommand
    {
        public DisableAccountCommand(IAcSession acSession, Guid accountId)
        {
            this.AcSession = acSession;
            this.AccountId = accountId;
        }

        public IAcSession AcSession { get; private set; }

        public Guid AccountId { get; private set; }
    }
}
