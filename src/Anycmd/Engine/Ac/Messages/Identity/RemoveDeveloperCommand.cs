
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Commands;
    using System;

    public class RemoveDeveloperCommand: Command, IAnycmdCommand
    {
        public RemoveDeveloperCommand(IAcSession userSession, Guid accountId)
        {
            this.AcSession = userSession;
            this.AccountId = accountId;
        }

        public IAcSession AcSession { get; private set; }

        public Guid AccountId { get; private set; }
    }
}
