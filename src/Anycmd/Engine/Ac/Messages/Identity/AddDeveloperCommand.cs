
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Anycmd.Commands;
    using System;

    public class AddDeveloperCommand : Command, IAnycmdCommand
    {
        public AddDeveloperCommand(IAcSession userSession, Guid accountId)
        {
            this.AcSession = userSession;
            this.AccountId = accountId;
        }

        public IAcSession AcSession { get; private set; }
        public Guid AccountId { get; private set; }
    }
}
