
namespace Anycmd.Engine.Ac.Messages.Infra
{
    using System;

    public class RemoveEntityTypeCommand : RemoveEntityCommand
    {
        public RemoveEntityTypeCommand(IAcSession acSession, Guid entityTypeId)
            : base(acSession, entityTypeId)
        {

        }
    }
}
