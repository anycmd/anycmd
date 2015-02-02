
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveResourceTypeCommand : RemoveEntityCommand
    {
        public RemoveResourceTypeCommand(IAcSession userSession, Guid resourceTypeId)
            : base(userSession, resourceTypeId)
        {

        }
    }
}
