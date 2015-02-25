
namespace Anycmd.Engine.Ac.Accounts
{
    using Anycmd.Commands;
    using System;

    public class AddDeveloperCommand : Command, IAnycmdCommand
    {
        public AddDeveloperCommand(IAcSession acSession, Guid accountId)
        {
            this.AcSession = acSession;
            this.AccountId = accountId;
        }

        public IAcSession AcSession { get; private set; }
        public Guid AccountId { get; private set; }
    }
}
