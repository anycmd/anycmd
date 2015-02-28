
namespace Anycmd.Engine.Ac.EntityTypes
{
    using Messages;
    using System;

    public sealed class RemoveEntityTypeCommand : RemoveEntityCommand
    {
        public RemoveEntityTypeCommand(IAcSession acSession, Guid entityTypeId)
            : base(acSession, entityTypeId)
        {

        }
    }
}
