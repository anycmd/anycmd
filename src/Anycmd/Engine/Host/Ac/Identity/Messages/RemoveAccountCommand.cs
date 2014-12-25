
namespace Anycmd.Engine.Host.Ac.Identity.Messages
{
    using Commands;
    using Model;
    using System;

    public class RemoveAccountCommand : RemoveEntityCommand, ISysCommand
    {
        public RemoveAccountCommand(Guid accountId)
            : base(accountId)
        {

        }
    }
}
