
namespace Anycmd.Engine.Ac.EntityTypes
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
