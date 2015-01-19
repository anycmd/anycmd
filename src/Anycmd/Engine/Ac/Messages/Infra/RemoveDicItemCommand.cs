
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveDicItemCommand : RemoveEntityCommand
    {
        public RemoveDicItemCommand(IUserSession userSession, Guid dicItemId)
            : base(userSession, dicItemId)
        {

        }
    }
}
