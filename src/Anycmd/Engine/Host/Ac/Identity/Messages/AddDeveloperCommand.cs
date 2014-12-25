
namespace Anycmd.Engine.Host.Ac.Identity.Messages
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
