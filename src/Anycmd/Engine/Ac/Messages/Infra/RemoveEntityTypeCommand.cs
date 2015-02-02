
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveEntityTypeCommand : RemoveEntityCommand
    {
        public RemoveEntityTypeCommand(IAcSession userSession, Guid entityTypeId)
            : base(userSession, entityTypeId)
        {

        }
    }
}
