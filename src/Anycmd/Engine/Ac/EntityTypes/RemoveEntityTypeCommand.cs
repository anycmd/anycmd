
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;
    using System;

    public class RemoveEntityTypeCommand : RemoveEntityCommand
    {
        public RemoveEntityTypeCommand(IAcSession acSession, Guid entityTypeId)
            : base(acSession, entityTypeId)
        {

        }
    }
}
