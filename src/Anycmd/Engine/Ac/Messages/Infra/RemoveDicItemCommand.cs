
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveDicItemCommand : RemoveEntityCommand
    {
        public RemoveDicItemCommand(IAcSession userSession, Guid dicItemId)
            : base(userSession, dicItemId)
        {

        }
    }
}
