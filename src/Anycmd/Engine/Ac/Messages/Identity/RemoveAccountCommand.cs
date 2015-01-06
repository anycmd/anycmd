
namespace Anycmd.Engine.Ac.Messages.Identity
{
    using System;

    public class RemoveAccountCommand : RemoveEntityCommand, IAnycmdCommand
    {
        public RemoveAccountCommand(Guid accountId)
            : base(accountId)
        {

        }
    }
}
