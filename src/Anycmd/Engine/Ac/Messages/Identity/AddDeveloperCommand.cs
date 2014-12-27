
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using Anycmd.Commands;
    using System;

    public class AddDeveloperCommand : Command, ISysCommand
    {
        public AddDeveloperCommand(Guid accountId)
        {
            this.AccountId = accountId;
        }

        public Guid AccountId { get; private set; }
    }
}
