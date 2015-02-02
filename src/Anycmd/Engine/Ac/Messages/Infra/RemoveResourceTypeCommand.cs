
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveResourceTypeCommand : RemoveEntityCommand
    {
        public RemoveResourceTypeCommand(IAcSession acSession, Guid resourceTypeId)
            : base(acSession, resourceTypeId)
        {

        }
    }
}
