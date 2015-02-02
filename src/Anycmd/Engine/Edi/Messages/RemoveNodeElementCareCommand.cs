
namespace Anycmd.Engine.Edi.Messages
{
    using System;

    public class RemoveNodeElementCareCommand : RemoveEntityCommand
    {
        public RemoveNodeElementCareCommand(IAcSession acSession, Guid nodeElementCareId)
            : base(acSession, nodeElementCareId)
        {

        }
    }
}
